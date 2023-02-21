using System;
using Abstracts;
using Asteroid;
using UnityEngine;


namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(AsteroidConfig), menuName = "Configs/Asteroids/" + nameof(AsteroidConfig))]
    public abstract class AsteroidConfig : ScriptableObject, IIdentityItem<string>
    {
        [field: Header("Base Settings")]
        [field: SerializeField] public string Id { get; private set; } = Guid.NewGuid().ToString();

        [NonSerialized] public AsteroidConfigType ConfigType;

        [field: SerializeField, Min(0.1f)] public float SpawnChance { get; private set; }
    }
}