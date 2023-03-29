using Gameplay.Minimap;
using Scriptables;
using UI.Services;
using UnityEngine;
using Zenject;

namespace UI.Installers
{
    public sealed class GameUIServicesInstaller : MonoInstaller
    {
        [field: SerializeField] public MinimapCamera MinimapCamera { get; private set; }
        [field: SerializeField] public MinimapConfig MinimapConfig { get; private set; }

        public override void InstallBindings()
        {
            InstallPlayerInfoService();
            InstallPlayerStatusBarService();
            InstallPlayerSpeedometerService();
            InstallLevelInfoService();
            InstallMinimapService();
            
            InstallEnemyStatusBarService();
            //TODO GameEventUIService
        }

        private void InstallPlayerInfoService()
        {
            Container
                .BindInterfacesAndSelfTo<PlayerInfoService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InstallPlayerStatusBarService()
        {
            Container
                .BindInterfacesAndSelfTo<PlayerStatusBarService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InstallPlayerSpeedometerService()
        {
            Container
                .BindInterfacesAndSelfTo<PlayerSpeedometerService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InstallLevelInfoService()
        {
            Container
                .BindInterfacesAndSelfTo<LevelInfoService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InstallMinimapService()
        {
            Container
                .Bind<MinimapCamera>()
                .FromInstance(MinimapCamera)
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<MinimapConfig>()
                .FromInstance(MinimapConfig)
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<MinimapService>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallEnemyStatusBarService()
        {
            Container
                .BindInterfacesAndSelfTo<EnemyStatusBarService>()
                .AsSingle()
                .NonLazy();
        }
    }
}