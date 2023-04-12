using System;
using System.Collections.Generic;
using Gameplay.Space.Planets;
using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using UnityEngine;
using Utilities.Mathematics;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class PlanetSystemEffectFactory : PlaceholderFactory<Transform, PlanetSystemConfig, PlanetSystemEffect>
    {
        private readonly PlanetFactory _planetFactory;

        public PlanetSystemEffectFactory(PlanetFactory planetFactory)
        {
            _planetFactory = planetFactory;
        }

        public override PlanetSystemEffect Create(Transform spaceObjectTransform, PlanetSystemConfig config)
        {
            List<Planet> planets = new();
            int planetCount = RandomPicker.PickRandomBetweenTwoValues(config.MinPlanetCount, config.MaxPlanetCount);
            float[] planetOrbits = GetPlanetOrbitList(planetCount, config.MinOrbit, config.MaxOrbit, spaceObjectTransform.localScale);
            for (int i = 0; i < planetCount; i++)
            {
                var planetConfig = RandomPicker.PickOneElementByWeights(config.PlanetConfigs);
                Vector2 planetPosition = GetPlanetPosition(spaceObjectTransform, planetOrbits[i], planetConfig);
                var planet = _planetFactory.Create(planetPosition, planetConfig, spaceObjectTransform);
                planets.Add(planet);
            }

            return new PlanetSystemEffect(planets);
        }

        private float[] GetPlanetOrbitList(int planetCount, float minOrbit, float maxOrbit, Vector3 spaceObjectScale)
        {
            if (planetCount == 0) return Array.Empty<float>();

            var spaceObjectSize = spaceObjectScale.x;
            var orbits = new float[planetCount];
            float realMinOrbit = spaceObjectSize / 2 + minOrbit;
            float realMaxOrbit = spaceObjectSize / 2 + maxOrbit;

            if (planetCount == 1)
            {
                orbits[0] = RandomPicker.PickRandomBetweenTwoValues(realMinOrbit, realMaxOrbit);
                return orbits;
            }

            float orbitChunk = realMaxOrbit - realMinOrbit / planetCount;
            
            for (int i = 0; i < planetCount; i++)
            {
                float minChunkOrbit = realMinOrbit + i * orbitChunk;
                float maxChunkOrbit = realMinOrbit + (i + 1) * orbitChunk;
                orbits[i] = RandomPicker.PickRandomBetweenTwoValues(minChunkOrbit, maxChunkOrbit);
            }

            return orbits;
        }

        private Vector2 GetPlanetPosition(Transform spaceObjectTransform, float planetOrbit, PlanetConfig planetConfig)
        {
            var spaceObjectSize = spaceObjectTransform.localScale.x;
            var planetSpawnPosition = spaceObjectTransform.position + new Vector3(0, spaceObjectSize + planetOrbit + planetConfig.MaxSize / 2, 0);
            return planetSpawnPosition;
        }
    }
}