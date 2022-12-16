using UnityEngine;
using Utilities.Reactive.SubscriptionProperty;

namespace Gameplay.Input
{
    public abstract class BaseInputView : MonoBehaviour
    {
        private SubscribedProperty<Vector3> _mousePositionInput;
        
        private SubscribedProperty<float> _verticalAxisInput;

        private SubscribedProperty<bool> _primaryFireInput;
        private SubscribedProperty<bool> _changeWeaponInput;
        private SubscribedProperty<bool> _nextLevelInput;
        
        public virtual void Init(
            SubscribedProperty<Vector3> mousePositionInput,
            SubscribedProperty<float> verticalMove,
            SubscribedProperty<bool> primaryFireInput,
            SubscribedProperty<bool> changeWeaponInput,
            SubscribedProperty<bool> nextLevelInput)
        {
            _mousePositionInput = mousePositionInput;
            _verticalAxisInput = verticalMove;
            _primaryFireInput = primaryFireInput;
            _changeWeaponInput = changeWeaponInput;
            _nextLevelInput = nextLevelInput;
        }

        protected virtual void OnMousePositionInput(Vector3 value)
            => _mousePositionInput.Value = value;

        protected virtual void OnVerticalInput(float value) 
            => _verticalAxisInput.Value = value;

        protected virtual void OnPrimaryFireInput(bool value)
            => _primaryFireInput.Value = value;
        
        protected virtual void OnChangeWeaponInput(bool value)
            => _changeWeaponInput.Value = value;

        protected virtual void OnNextLevelInputInput(bool value)
            => _nextLevelInput.Value = value;
    }
}