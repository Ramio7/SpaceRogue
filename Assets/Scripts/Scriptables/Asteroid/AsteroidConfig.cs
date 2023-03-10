using System;
using Abstracts;
using Asteroid;
using UnityEngine;


namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(AsteroidConfig), menuName = "Configs/Asteroids/" + nameof(AsteroidConfig))]
    public abstract class AsteroidConfig : ScriptableObject
    {
        [field: Header("Base Settings")]

        [NonSerialized] public AsteroidConfigType ConfigType;
    }
}