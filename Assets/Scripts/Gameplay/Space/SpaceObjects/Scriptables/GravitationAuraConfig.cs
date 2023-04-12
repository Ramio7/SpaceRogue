using Abstracts;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views;
using UnityEngine;

namespace Gameplay.Space.SpaceObjects.Scriptables
{
    [CreateAssetMenu(fileName = nameof(GravitationAuraConfig), menuName = "Configs/Space/SpaceObjectEffects/" + nameof(GravitationAuraConfig))]
    public class GravitationAuraConfig : SpaceObjectEffectConfig
    {
        [field: SerializeField] public GravitationAuraEffectView Prefab { get; private set; }
        [field: SerializeField, Range(1f, 5f)] public float Radius { get; private set; } = 1.5f;
        
        [field: SerializeField, Min(0.1f)] public float GravityForce { get; private set; } = 0.1f;
        
        [field: SerializeField, Min(0.0f)]
        [field: Tooltip("If gravity force is 10 and gravity variation is 5, result gravity will vary from 5 to 15.")] 
        public float GravityVariation { get; private set; } = 0.1f;
        
        [field: SerializeField, Min(0.1f)]
        [field: Tooltip("This scales the distance between objects - if this parameter is 5, the pull will be 5x slower, and if it is 0.5, the pull is 2x faster")] 
        public float GravityDistanceScale { get; private set; } = 1.0f;
        
        [field: SerializeField, 
                Tooltip("Constant - the same attraction in the entire zone. Linear and square - change in attraction from the edge to the center.")] 
        public EffectorForceMode2D GravityMode { get; private set; } = EffectorForceMode2D.Constant;
        
        public GravitationAuraConfig() => Type = SpaceObjectEffectType.GravitationAura;
    }
}