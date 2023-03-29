using System;
using Gameplay.Abstracts;
using Gameplay.Survival.DamageImmunityFrame;
using Gameplay.Survival.Health;
using Gameplay.Survival.Shield;
using Zenject;

namespace Gameplay.Survival
{
    public sealed class EntitySurvivalFactory : PlaceholderFactory<EntityView, EntitySurvivalConfig, EntitySurvival>
    {
        private readonly EntityHealthFactory _entityHealthFactory;
        private readonly EntityShieldFactory _entityShieldFactory;
        private readonly EntityDamageImmunityFrameFactory _entityDamageImmunityFrameFactory;

        public EntitySurvivalFactory(
            EntityHealthFactory entityHealthFactory,
            EntityShieldFactory entityShieldFactory,
            EntityDamageImmunityFrameFactory entityDamageImmunityFrameFactory)
        {
            _entityHealthFactory = entityHealthFactory;
            _entityShieldFactory = entityShieldFactory;
            _entityDamageImmunityFrameFactory = entityDamageImmunityFrameFactory;
        }

        public override EntitySurvival Create(EntityView view, EntitySurvivalConfig config)
        {
            if (config.Health is null) throw new ArgumentNullException(nameof(config.Health));
            var entityHealth = _entityHealthFactory.Create(config.Health);
            var entityShield = config.Shield is null 
                ? null 
                : _entityShieldFactory.Create(config.Shield);
            var entityDamageImmunityFrame = config.DamageImmunityFrame is null
                ? null
                : _entityDamageImmunityFrameFactory.Create(config.DamageImmunityFrame);

            return new EntitySurvival(view, entityHealth, entityShield, entityDamageImmunityFrame);
        }
    }
}