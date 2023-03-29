using Abstracts;
using Gameplay.Abstracts;

namespace Gameplay.Damage
{
    public sealed class DamageModel
    {
        public float MinDamage { get; }
        public float MaxDamage { get; }
        public EntityType EntityType { get; }

        public DamageModel(float minDamage, float maxDamage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            EntityType = EntityType.None;
        }
        
        public DamageModel(float damageAmount)
        {
            MinDamage = damageAmount;
            MaxDamage = damageAmount;
            EntityType = EntityType.None;
        }

        public DamageModel(float minDamage, float maxDamage, EntityType entityType)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            EntityType = entityType;
        }
        
        public DamageModel(float damageAmount, EntityType entityType)
        {
            MinDamage = damageAmount;
            MaxDamage = damageAmount;
            EntityType = entityType;
        }
    }
}