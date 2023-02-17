using Asteroid;
using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Scriptables.Asteroid;
using Scriptables.Health;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = nameof(SingleAsteroidConfig), menuName = "Configs/Asteroids/" + nameof(SingleAsteroidConfig))]
[Serializable]
public sealed class SingleAsteroidConfig : AsteroidConfig
{
    public SingleAsteroidConfig() => ConfigType = AsteroidConfigType.SingleAsteroidConfig;
    [field: SerializeField] public AsteroidType AsteroidType { get; private set; }
    [field: SerializeField] public AsteroidView Prefab { get; private set; }
    [field: SerializeField, Min(0.1f)] public float CollisionDamageAmount { get; private set; }
    [field: SerializeField] public bool SpawnAsteroidCloudOnDestroy { get; private set; }

    [field: Header("Special Settings")]
    [field: SerializeField] public AsteroidSizeConfig Size { get; private set; }
    [field: SerializeField] public AsteroidBehaviourConfig Behaviour { get; set; }
    [field: SerializeField] public HealthConfig Health { get; private set; }
    [field: SerializeField] public AsteroidCloudConfig Cloud { get; private set; }
}
