using System;
using UI.Common;
using UnityEngine;

namespace UI.Game
{
    public sealed class DestroyPlayerMessageView : MonoBehaviour
    {
        [field: SerializeField] public ButtonView DestroyPlayerButton;

        public void Init(Action onClickAction)
        {
            DestroyPlayerButton.Init(onClickAction);
        }
    } 
}
