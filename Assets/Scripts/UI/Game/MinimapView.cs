using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public sealed class MinimapView : MonoBehaviour
    {
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public Image MinimapMask { get; private set; }

        public void SetAlpha(float value) => CanvasGroup.alpha = value;
        public void SetColor(Color color) => MinimapMask.color = color;
    }
}