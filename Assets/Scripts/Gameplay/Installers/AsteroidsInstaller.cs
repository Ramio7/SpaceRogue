using Asteroids;
using Gameplay.Asteroids;
using Gameplay.Asteroids.Factories;
using Gameplay.Asteroids.Scriptables;
using Gameplay.Space.Generator;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class AsteroidsInstaller : MonoInstaller
    {
        [field: SerializeField] public AsteroidSpawnConfig AsteroidSpawnConfig { get; private set; }
        [field: SerializeField] public AsteroidsPool Pool { get; private set; }

        public override void InstallBindings()
        {
            InstallAsteroidsPool();
            InstallAsteroidViewFactory();
            InstallAsteroidFactory();
            InstallAsteroidsInSpaceFactory();
            InstallAsteroidSpawner();
        }

        private void InstallAsteroidsPool()
        {
            Container
                .Bind<AsteroidsPool>()
                .FromInstance(Pool)
                .AsSingle();
        }

        private void InstallAsteroidViewFactory()
        {
            Container
                .BindFactory<Vector2, AsteroidConfig, AsteroidView, AsteroidViewFactory>()
                .AsSingle();
        }

        private void InstallAsteroidFactory()
        {
            Container
                .BindFactory<Vector2, AsteroidConfig, Asteroid, AsteroidFactory>()
                .AsSingle();
        }

        private void InstallAsteroidsInSpaceFactory()
        {
            Container
                .Bind<AsteroidSpawnConfig>()
                .FromInstance(AsteroidSpawnConfig)
                .WhenInjectedInto<AsteroidsInSpaceFactory>();

            Container
                .BindFactory<int, SpawnPointsFinder, AsteroidsInSpace, AsteroidsInSpaceFactory>()
                .AsSingle();
        }

        private void InstallAsteroidSpawner()
        {
            Container
                .BindFactory<SpawnPointsFinder, AsteroidSpawner, AsteroidSpawnerFactory>()
                .AsSingle();
        }
    }
}