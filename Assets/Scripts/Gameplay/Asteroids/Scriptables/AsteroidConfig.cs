using Gameplay.Asteroids.Enums;
using Gameplay.Survival;
using UnityEngine;

namespace Gameplay.Asteroids.Scriptables
{
    [CreateAssetMenu(fileName = nameof(AsteroidConfig), menuName = "Configs/Asteroids/" + nameof(AsteroidConfig))]
    public class AsteroidConfig : ScriptableObject
    {
        [field: SerializeField] public AsteroidView Prefab { get; private set; }
        [field: SerializeField] public EntitySurvivalConfig SurvivalConfig { get; private set; }
        [field: SerializeField] public AsteroidType Type { get; private set; }
        [field: SerializeField] public AsteroidSizeType SizeType { get; private set; }
        [field: SerializeField] public AsteroidMoveType MoveType { get; private set; }
        [field: SerializeField, Min(0.0f)] public float DirectionScattering { get; private set; } = 0.0f;
        [field: SerializeField, Min(0.0f)] public float StartingSpeed { get; private set; } = 4.0f;
        [field: SerializeField, Min(0.0f)] public float MaxSpawnRadius { get; private set; } = 100.0f;
        [field: SerializeField, Min(0.0f)] public float AsteroidLifeTime { get; private set; } = 20.0f;
        [field: SerializeField, Min(0.0f)] public float Damage { get; private set; } = 1.0f;
    }
}