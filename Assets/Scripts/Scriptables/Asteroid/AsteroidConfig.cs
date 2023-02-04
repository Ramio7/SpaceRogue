using System;
using Abstracts;
using Asteroid;
using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Scriptables.Health;
using UnityEngine;

namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(AsteroidConfig), menuName = "Configs/Asteroids/" + nameof(AsteroidConfig))]
    public class AsteroidConfig : ScriptableObject, IIdentityItem<string>
    {
        [field: Header("Base Settings")]
        [field: SerializeField] public string Id { get; private set; } = Guid.NewGuid().ToString();
        [field: SerializeField] public AsteroidType Type { get; private set; }
        [field: SerializeField] public float SpawnChanceWeight;
        [field: SerializeField, Min(0.1f)] public float CollisionDamageAmount { get; private set; }
        [field: SerializeField, Min(1f)] public Vector3 AsteroidSize { get; private set; }
        [field: SerializeField] public AsteroidView Prefab { get; private set; }
        [field: SerializeField] public AsteroidBehaviourConfig Behaviour { get; private set; }
        [field: SerializeField] public HealthConfig Health { get; private set; }
    }
}