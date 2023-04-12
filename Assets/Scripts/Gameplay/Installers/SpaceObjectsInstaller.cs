using UnityEngine;
using Zenject;
using Gameplay.Pooling;
using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects;
using Gameplay.Space.Factories;
using Gameplay.Space.Generator;
using Gameplay.Space.Planets;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views;

namespace Gameplay.Installers
{
    public sealed class SpaceObjectsInstaller : MonoInstaller
    {
        [field: SerializeField] public SpaceObjectSpawnConfig SpaceObjectSpawnConfig { get; private set; }
        [field: SerializeField] public SpaceObjectsPool SpaceObjectsPool { get; private set; }

        public override void InstallBindings()
        {
            InstallSpaceObjectsPool();
            InstallSpaceObjectEffectFactories();
            InstallSpaceObjectFactories();
            InstallSpace();
        }

        private void InstallSpaceObjectsPool()
        {
            Container
                .Bind<SpaceObjectsPool>()
                .FromInstance(SpaceObjectsPool)
                .AsSingle();
        }

        private void InstallPlanetFactories()
        {
            Container
               .BindFactory<Vector2, PlanetConfig, PlanetView, PlanetViewFactory>()
               .AsSingle();

            Container
               .BindFactory<PlanetView, PlanetConfig, Transform, PlanetMovement, PlanetMovementFactory>()
               .AsSingle();

            Container
               .BindFactory<Vector2, PlanetConfig, Transform, Planet, PlanetFactory>()
               .AsSingle();

            Container
               .BindFactory<Transform, PlanetSystemConfig, PlanetSystemEffect, PlanetSystemEffectFactory>()
               .AsSingle();
        }

        private void InstallSpaceObjectEffectFactories()
        {
            InstallGravitationAuraEffect();
            InstallDamageAuraEffect();
            InstallDamageOnTouchEffect();
            InstallPlanetFactories();

            Container
                .BindIFactory<Transform, SpaceObjectEffectConfig, SpaceObjectEffect>()
                .FromFactory<SpaceObjectEffectFactory>();
        }

        private void InstallGravitationAuraEffect()
        {
            Container
                .BindFactory<Transform, GravitationAuraConfig, GravitationAuraEffect, GravitationAuraFactory>()
                .AsSingle();

            Container
                .BindFactory<Transform, GravitationAuraConfig, GravitationAuraEffectView, GravitationAuraViewFactory>()
                .AsSingle();
        }

        private void InstallDamageAuraEffect()
        {
            Container
                .BindFactory<Transform, DamageAuraConfig, DamageAuraEffect, DamageAuraFactory>()
                .AsSingle();

            Container
                .BindFactory<Transform, DamageAuraConfig, DamageAuraView, DamageAuraViewFactory>()
                .AsSingle();
        }

        public void InstallDamageOnTouchEffect()
        {
            Container
                .BindFactory<Transform, DamageOnTouchConfig, DamageOnTouchEffect, DamageOnTouchFactory>()
                .AsSingle();

            Container
                .BindFactory<Transform, DamageOnTouchConfig, DamageOnTouchEffectView, DamageOnTouchViewFactory>()
                .AsSingle();
        }

        private void InstallSpaceObjectFactories()
        {
            Container
                .BindFactory<Vector2, SpaceObjectConfig, SpaceObjectView, SpaceObjectViewFactory>()
                .AsSingle();

            Container
                .BindFactory<Vector2, SpaceObjectConfig, SpaceObject, SpaceObjectFactory>()
                .AsSingle();
        }

        private void InstallSpace()
        {
            Container
                .Bind<SpaceObjectSpawnConfig>()
                .FromInstance(SpaceObjectSpawnConfig)
                .AsSingle();

            Container
                .BindFactory<int, SpawnPointsFinder, Space.Space, SpaceFactory>()
                .AsSingle();
        }
    }
}