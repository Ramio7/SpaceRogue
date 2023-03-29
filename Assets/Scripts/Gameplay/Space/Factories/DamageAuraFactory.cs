using Gameplay.Damage;
using Gameplay.Mechanics.Timer;
using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using UnityEngine;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class DamageAuraFactory : PlaceholderFactory<Transform, DamageAuraConfig, DamageAuraEffect>
    {
        private readonly DamageAuraViewFactory _viewFactory;
        private readonly TimerFactory _timerFactory;

        public DamageAuraFactory(DamageAuraViewFactory viewFactory, TimerFactory timerFactory)
        {
            _viewFactory = viewFactory;
            _timerFactory = timerFactory;
        }

        public override DamageAuraEffect Create(Transform transform, DamageAuraConfig config)
        {
            var view = _viewFactory.Create(transform, config);
            var damageModel = new DamageModel(config.Damage);
            var timer = _timerFactory.Create(config.DamageInterval);
            return new DamageAuraEffect(damageModel, view, timer);
        }
    }
}