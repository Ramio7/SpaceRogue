using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(MinimapConfig), menuName = "Configs/" + nameof(MinimapConfig))]
    public sealed class MinimapConfig : ScriptableObject
    {
        [field: SerializeField] public float MinimapHeight { get; private set; } = 250;
        [field: SerializeField] public float MinimapCameraSize { get; private set; } = 150;
        [field: SerializeField] public Color MinimapColor { get; private set; } = new Color32(32, 48, 33, 255);
        [field: SerializeField, Range(0, 1)] public float MinimapAlpha { get; private set; } = 0.75f;
    }
}