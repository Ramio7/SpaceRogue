using Gameplay.Space.SpaceObjects.Scriptables;
using UnityEngine;
using Utilities.Mathematics;
using Random = System.Random;

namespace Gameplay.Space.Generator
{
    public sealed class MapGenerator
    {
        private const int DefaultMooreCount = 4;
        private const int DefaultVonNeumannCount = 2;
        private const float NoiseScale = 0.1f;

        private readonly SpaceConfig _spaceConfig;

        public int[,] BorderMap { get; private set; }
        public int[,] NebulaMap { get; private set; }

        public MapGenerator(SpaceConfig spaceConfig)
        {
            _spaceConfig = spaceConfig;
        }

        public void Generate()
        {
            BorderMap = CreateBorderMap(_spaceConfig.WidthMap, _spaceConfig.HeightMap, _spaceConfig.OuterBorder);
            NebulaMap = CreateNebulaMap(_spaceConfig.WidthMap, _spaceConfig.HeightMap, _spaceConfig.InnerBorder, NoiseScale,
                _spaceConfig.RandomType, _spaceConfig.Chance, _spaceConfig.SmoothMapType, _spaceConfig.FactorSmooth);
        }

        private int[,] CreateBorderMap(int widthMap, int heightMap, int outerBorder)
        {
            var map = new int[widthMap + 2 * outerBorder, heightMap + 2 * outerBorder];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (x <= outerBorder - 1
                        || x >= widthMap + outerBorder
                        || y <= outerBorder - 1
                        || y >= heightMap + outerBorder)
                    {
                        map[x, y] = (int)CellType.Border;
                    }
                }
            }

            return map;
        }

        private int[,] CreateNebulaMap(int widthMap, int heightMap, int innerBorder, float noiseScale,
            RandomType randomType, float chance, SmoothMapType smoothMapType, int factorSmooth)
        {
            var map = new int[widthMap, heightMap];

            RandomFillMap(randomType, map, innerBorder, chance, noiseScale);
            SmoothMap(smoothMapType, factorSmooth, map, 1, false);

            return map;
        }

        private void RandomFillMap(RandomType randomType, int[,] map, int innerBorder, 
            float chance, float noiseScale)
        {
            var pseudoRandom = new Random();

            switch (randomType)
            {
                case RandomType.Random:
                    StandardRandomMapFill(map, innerBorder, pseudoRandom, chance);
                    break;
                case RandomType.PerlinNoise:
                    PerlinNoiseMapFill(map, innerBorder, pseudoRandom, chance, noiseScale);
                    break;
            }
        }

        private void StandardRandomMapFill(int[,] map, int innerBorder, Random pseudoRandom, float chance)
        {
            for (int x = innerBorder; x < map.GetLength(0) - innerBorder; x++)
            {
                for (int y = innerBorder; y < map.GetLength(1) - innerBorder; y++)
                {
                    map[x, y] = RandomPicker.TakeChance(chance) ? (int)CellType.Obstacle : (int)CellType.None;
                }
            }
        }

        private void PerlinNoiseMapFill(int[,] map, int innerBorder, Random pseudoRandom, 
            float chance, float noiseScale)
        {
            var xOffset = pseudoRandom.Next(-map.GetLength(0) / 2, map.GetLength(0) / 2);
            var yOffset = pseudoRandom.Next(-map.GetLength(1) / 2, map.GetLength(1) / 2);

            for (int x = innerBorder; x < map.GetLength(0) - innerBorder; x++)
            {
                for (int y = innerBorder; y < map.GetLength(1) - innerBorder; y++)
                {
                    var noise = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
                    map[x, y] = noise >= 0.99f - chance ? (int)CellType.Obstacle : (int)CellType.None;
                }
            }
        }

        private void SmoothMap(SmoothMapType smoothMapType, int factorSmooth, int[,] map, int radius, 
            bool edgesAreWalls)
        {
            for (int i = 0; i < factorSmooth; i++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        var neighbourCount = GetSurroundingNeighbourCount(smoothMapType, map, 
                            x, y, radius, edgesAreWalls, out int defaultCount);

                        if (neighbourCount > defaultCount)
                        {
                            map[x, y] = (int)CellType.Obstacle;
                        }
                        else if (neighbourCount < defaultCount)
                        {
                            map[x, y] = (int)CellType.None;
                        }
                    }
                }
            }
        }

        private int GetSurroundingNeighbourCount(SmoothMapType smoothMapType, int[,] map, 
            int gridX, int gridY, int radius, bool edgesAreWalls, out int defaultCount)
        {
            var count = 0;
            defaultCount = 0;

            switch (smoothMapType)
            {
                case SmoothMapType.MooreNeighborhood:
                    count = MooreNeighborhoodCount(map, gridX, gridY, radius, edgesAreWalls);
                    defaultCount = DefaultMooreCount;
                    break;
                case SmoothMapType.VonNeumannNeighborhood:
                    count = VonNeumannNeighborhoodCount(map, gridX, gridY, edgesAreWalls);
                    defaultCount = DefaultVonNeumannCount;
                    break;
            }

            return count;
        }

        private int MooreNeighborhoodCount(int[,] map, int gridX, int gridY, int radius, bool edgesAreWalls)
        {
            var neighbourCount = 0;

            for (int neighbourX = gridX - radius; neighbourX <= gridX + radius; neighbourX++)
            {
                for (int neighbourY = gridY - radius; neighbourY <= gridY + radius; neighbourY++)
                {
                    bool isInsideMap = 
                        neighbourX >= 0 && neighbourX < map.GetLength(0)
                        && neighbourY >= 0 && neighbourY < map.GetLength(1);

                    if (isInsideMap)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            neighbourCount += map[neighbourX, neighbourY] > 0 ? 1 : 0;
                        }
                    }
                    else if (edgesAreWalls)
                    {
                        neighbourCount++;
                    }
                }
            }

            return neighbourCount;
        }

        private int VonNeumannNeighborhoodCount(int[,] map, int gridX, int gridY, bool edgesAreWalls)
        {
            var neighbourCount = 0;

            bool isEdge = 
                gridX - 1 == 0 || gridX + 1 == map.GetLength(0)
                || gridY - 1 == 0 || gridY + 1 == map.GetLength(1);

            if (edgesAreWalls && isEdge)
            {
                neighbourCount++;
            }

            if (gridX - 1 > 0)
            {
                neighbourCount += map[gridX - 1, gridY] > 0 ? 1 : 0;
            }

            if (gridY - 1 > 0)
            {
                neighbourCount += map[gridX, gridY - 1] > 0 ? 1 : 0;
            }

            if (gridX + 1 < map.GetLength(0))
            {
                neighbourCount += map[gridX + 1, gridY] > 0 ? 1 : 0;
            }

            if (gridY + 1 < map.GetLength(1))
            {
                neighbourCount += map[gridX, gridY + 1] > 0 ? 1 : 0;
            }

            return neighbourCount;
        }
    }
}