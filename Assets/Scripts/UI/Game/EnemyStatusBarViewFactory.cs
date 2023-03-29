using Zenject;
using Gameplay.Survival;

namespace UI.Game
{
    public sealed class EnemyStatusBarViewFactory : PlaceholderFactory<EntitySurvival, EnemyHealthBarsView, HealthStatusBarView>
    {
        private readonly HealthShieldStatusBarView _healthShieldStatusBarView;
        private readonly HealthStatusBarView _healthStatusBarView;
        private readonly DiContainer _diContainer;

        public EnemyStatusBarViewFactory(HealthShieldStatusBarView healthShieldStatusBarView, HealthStatusBarView healthStatusBarView, DiContainer diContainer)
        {
            _healthShieldStatusBarView = healthShieldStatusBarView;
            _healthStatusBarView = healthStatusBarView;
            _diContainer = diContainer;
        }

        public override HealthStatusBarView Create(EntitySurvival survival, EnemyHealthBarsView barsView)
        {
            if (survival.EntityShield != null)
            {
                return _diContainer
                    .InstantiatePrefabForComponent<HealthShieldStatusBarView>(_healthShieldStatusBarView, barsView.transform);
            }

            return _diContainer
                .InstantiatePrefabForComponent<HealthStatusBarView>(_healthStatusBarView, barsView.transform);
        }
    }
}