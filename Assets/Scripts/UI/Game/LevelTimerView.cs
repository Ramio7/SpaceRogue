using UI.Common;
using UnityEngine;

namespace UI.Game
{
    public sealed class LevelTimerView : MonoBehaviour
    {
        [field: SerializeField] public TextView TimerTextView { get; private set; }

        public void Init(string text) => TimerTextView.Init(text);
        public void UpdateText(string text) => TimerTextView.UpdateText(text);
    }
}