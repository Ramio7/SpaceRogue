using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(MenuBackgroundConfig), menuName = "Configs/Background/" + nameof(MenuBackgroundConfig))]
    public sealed class MenuBackgroundConfig : ScriptableObject
    {
        [field: SerializeField] public float CameraCoefficient { get; private set; }
        [field: SerializeField] public float BackCoefficient { get; private set; }
        [field: SerializeField] public float MidCoefficient { get; private set; }
        [field: SerializeField] public float ForeCoefficient { get; private set; }

        [field: SerializeField] public float NebulaCoefficient { get; private set; }
    }
}