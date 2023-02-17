using Asteroid;
using System;
using UnityEngine;

namespace Gameplay.Asteroid.Behaviour
{
    [Serializable]
    public class AsteroidBehaviourConfig 
    {
        [field: SerializeField] public AsteroidMoveType AsteroidMoveType { get; set; }
        [field: SerializeField] public float AsteroidSpeed { get; private set; }
        [field: SerializeField] public float SpawnRadius { get; private set; }
        [field: SerializeField, Min(0.1f)] public float TargetDispersion { get; private set; }
        [field: SerializeField, Min(0)] public float AsteroidLifeTime { get; private set; }
    }
}