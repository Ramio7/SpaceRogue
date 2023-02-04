using System;
using UnityEngine;

namespace Scriptables.Asteroid
{
    [Serializable]
    public class SmallCloudConfig 
    {
        [field: Header("Asteroid Small Cloud _config")]
        [field: SerializeField] public int MinimumCount { get; private set; }
        [field: SerializeField] public int MaximumCount { get; private set; }
        [field: SerializeField] public int MinimumRadius { get; private set; }
        [field: SerializeField] public int MaximumRadius { get; private set; }

    }
}
