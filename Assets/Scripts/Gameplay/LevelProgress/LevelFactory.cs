using System;
using Asteroids;
using Gameplay.Asteroids;
using Gameplay.Asteroids.Factories;
using Gameplay.Enemy;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Gameplay.Services;
using Gameplay.Space.Factories;
using Gameplay.Space.Generator;
using Gameplay.Space.Obstacle;
using Scriptables;
using Zenject;

namespace Gameplay.LevelProgress
{
    public sealed class LevelFactory : PlaceholderFactory<int, Level>
    {
        private readonly LevelPresetsConfig _levelPresetsConfig;
        private readonly SpaceViewFactory _spaceViewFactory;
        private readonly MapGeneratorFactory _mapGeneratorFactory;
        private readonly LevelMapFactory _levelMapFactory;
        private readonly SpawnPointsFinderFactory _spawnPointsFinderFactory;
        private readonly SpaceObstacleFactory _spaceObstacleFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly SpaceFactory _spaceFactory;
        private readonly EnemyForcesFactory _enemyForcesFactory;
        private readonly AsteroidsInSpaceFactory _asteroidsInSpaceFactory;
        
        private LevelPreset _currentLevelPreset;

        public event Action<Level> LevelCreated = (_) => { };

        public LevelFactory(
            LevelPresetsConfig levelPresetsConfig,
            SpaceViewFactory spaceViewFactory,
            MapGeneratorFactory mapGeneratorFactory,
            LevelMapFactory levelMapFactory,
            SpawnPointsFinderFactory spawnPointsFinderFactory,
            SpaceObstacleFactory spaceObstacleFactory,
            PlayerFactory playerFactory,
            SpaceFactory spaceFactory,
            EnemyForcesFactory enemyForcesFactory,
            AsteroidsInSpaceFactory asteroidsInSpaceFactory)
        {
            _levelPresetsConfig = levelPresetsConfig;
            _spaceViewFactory = spaceViewFactory;
            _mapGeneratorFactory = mapGeneratorFactory;
            _levelMapFactory = levelMapFactory;
            _spawnPointsFinderFactory = spawnPointsFinderFactory;
            _spaceObstacleFactory = spaceObstacleFactory;
            _playerFactory = playerFactory;
            _spaceFactory = spaceFactory;
            _enemyForcesFactory = enemyForcesFactory;
            _asteroidsInSpaceFactory = asteroidsInSpaceFactory;
        }

        public override Level Create(int levelNumber)
        {
            _currentLevelPreset = PickRandomLevelPreset();
            var spaceView = _spaceViewFactory.Create();

            var map = _mapGeneratorFactory.Create(_currentLevelPreset.SpaceConfig);
            map.Generate();

            var levelMap = _levelMapFactory.Create(spaceView, _currentLevelPreset.SpaceConfig, map.BorderMap, map.NebulaMap);
            levelMap.Draw();
            var mapCameraSize = levelMap.GetMapCameraSize();

            var spawnPointsFinder = _spawnPointsFinderFactory.Create(map.NebulaMap, spaceView.NebulaTilemap);

            _spaceObstacleFactory.Create(spaceView.SpaceObstacleView, _currentLevelPreset.SpaceConfig.ObstacleForce);

            var player = _playerFactory.Create(spawnPointsFinder.GetPlayerSpawnPoint());

            var space = _spaceFactory.Create(_currentLevelPreset.SpaceConfig.SpaceObjectCount, spawnPointsFinder);

            var enemyForces = _enemyForcesFactory.Create(_currentLevelPreset.SpaceConfig.EnemyGroupCount, spawnPointsFinder);

            var asteroids = _asteroidsInSpaceFactory.Create(_currentLevelPreset.SpaceConfig.AsteroidsOnStartCount, spawnPointsFinder);
            asteroids.SpawnStartAsteroids();
            if (_currentLevelPreset.SpaceConfig.SpawnPlayerTargetedAsteroids) 
                asteroids.StartPlayerTargetedAsteroidsSpawn(_currentLevelPreset.SpaceConfig.PlayerTargetedAsteroidsSpawnDelay);

            var level = new Level(levelNumber, _currentLevelPreset.EnemiesCountToWin, mapCameraSize, player, enemyForces, space, asteroids);
            LevelCreated.Invoke(level);
            return level;
        }

        private LevelPreset PickRandomLevelPreset()
        {
            var index = new Random().Next(_levelPresetsConfig.Presets.Count);
            return _levelPresetsConfig.Presets[index];
        }
    }
}