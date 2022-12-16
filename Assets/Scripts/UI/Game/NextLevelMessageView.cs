using System;
using UI.Common;
using UnityEngine;

namespace UI.Game
{
    public sealed class NextLevelMessageView : MonoBehaviour
    {
        [field: SerializeField] public TextView LevelsNumber { get; private set; }
        [field: SerializeField] public ButtonView NextLevelButton;

        public void Init(float levelNumber, Action onClickAction)
        {
            LevelsNumber.Init(levelNumber.ToString());
            NextLevelButton.Init(onClickAction);
        }
    } 
}
