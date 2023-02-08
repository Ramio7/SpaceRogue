using Asteroid;
using System;
using UnityEngine;

[Serializable]
public class AsteroidSizeConfig
{
    [field: SerializeField] public AsteroidSizeType SizeType { get; private set; }
    [field: SerializeField] public Vector3 AsteroidScale { get; private set; }
}
