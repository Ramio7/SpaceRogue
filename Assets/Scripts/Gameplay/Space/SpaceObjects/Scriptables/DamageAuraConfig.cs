using Abstracts;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using UnityEngine;
using AreaEffectView = Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views.AreaEffectView;

namespace Gameplay.Space.SpaceObjects.Scriptables
{
    [CreateAssetMenu(fileName = nameof(DamageAuraConfig), menuName = "Configs/Space/SpaceObjectEffects/" + nameof(DamageAuraConfig))]
    public class DamageAuraConfig : SpaceObjectEffectConfig
    {
        [field: SerializeField, Header("AuraBody")] public AreaEffectView Prefab { get; private set; }
        [field: SerializeField, Min(1.01f), Header("Radius")] public float Radius { get; private set; } = 1.01f;
        [field: SerializeField, Min(0.1f), Header("AuraDamage")] public float Damage { get; private set; } = 0.1f;
        [field: SerializeField, Min(0.1f)] public float DamageInterval { get; private set; } = 0.1f;

        public DamageAuraConfig() => Type = SpaceObjectEffectType.DamageAura;
    }
}
