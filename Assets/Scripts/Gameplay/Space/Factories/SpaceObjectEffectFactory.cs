using System;
using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using UnityEngine;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class SpaceObjectEffectFactory : IFactory<Transform, SpaceObjectEffectConfig, SpaceObjectEffect>
    {
        private readonly PlanetSystemEffectFactory _planetSystemEffectFactory;
        private readonly GravitationAuraFactory _gravitationAuraFactory;
        private readonly DamageAuraFactory _damageAuraFactory;
        private readonly DamageOnTouchFactory _damageOnTouchFactory;

        public SpaceObjectEffectFactory(
            PlanetSystemEffectFactory planetSystemEffectFactory,
            GravitationAuraFactory gravitationAuraFactory,
            DamageAuraFactory damageAuraFactory,
            DamageOnTouchFactory damageOnTouchFactory)
        {
            _planetSystemEffectFactory = planetSystemEffectFactory;
            _gravitationAuraFactory = gravitationAuraFactory;
            _damageAuraFactory = damageAuraFactory;
            _damageOnTouchFactory = damageOnTouchFactory;
        }

        public SpaceObjectEffect Create(Transform spaceObjectTransform, SpaceObjectEffectConfig config)
        {
            return config.Type switch
            {
                SpaceObjectEffectType.None => new SpaceObjectEmptyEffect(),
                SpaceObjectEffectType.PlanetSystem => _planetSystemEffectFactory.Create(spaceObjectTransform, config as PlanetSystemConfig),
                SpaceObjectEffectType.DamageAura => _damageAuraFactory.Create(spaceObjectTransform, config as DamageAuraConfig),
                SpaceObjectEffectType.GravitationAura => _gravitationAuraFactory.Create(spaceObjectTransform, config as GravitationAuraConfig),
                SpaceObjectEffectType.DamageOnTouch => _damageOnTouchFactory.Create(spaceObjectTransform, config as DamageOnTouchConfig),
                _ => throw new ArgumentOutOfRangeException(nameof(config.Type), config.Type, "A not-existent game event type is provided")
            };
        }
    }
}

