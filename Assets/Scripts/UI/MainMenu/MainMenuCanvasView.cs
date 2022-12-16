using System;
using UI.Common;
using UnityEngine;

namespace UI.Game
{
    public sealed class MainMenuCanvasView : MonoBehaviour
    {
        [field: SerializeField] public ButtonView StartGameButton { get; private set; }
        [field: SerializeField] public ButtonView ExitGameButton { get; private set; }
        [field: SerializeField] public TextView PassedLevelsNumber { get; private set; }
        [field: SerializeField] public TextView RecordNumber { get; private set; }

        public void Init(Action startGameButton, Action exitGameButton, float levelsNumber, float levelsNumberRecord)
        {
            StartGameButton.Init(startGameButton);
            ExitGameButton.Init(exitGameButton);

            PassedLevelsNumber.Init(levelsNumber.ToString());
            RecordNumber.Init(levelsNumberRecord.ToString());
        }
    }
}