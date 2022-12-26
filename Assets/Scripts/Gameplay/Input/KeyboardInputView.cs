using UnityEngine;

namespace Gameplay.Input
{
    public sealed class KeyboardInputView : BaseInputView
    {
        [SerializeField] private float verticalAxisInputMultiplier;

        private const string Vertical = "Vertical";
        private const KeyCode PrimaryFire = KeyCode.Mouse0;
        private const KeyCode ChangeWeapon = KeyCode.Q;

        private void Start()
        {
            EntryPoint.SubscribeToUpdate(CheckVerticalInput);
            EntryPoint.SubscribeToUpdate(CheckFiringInput);
            EntryPoint.SubscribeToUpdate(CheckMousePositionInput);
            EntryPoint.SubscribeToUpdate(CheckChangeWeaponInput);
        }

        private void OnDestroy()
        {
            EntryPoint.UnsubscribeFromUpdate(CheckVerticalInput);
            EntryPoint.UnsubscribeFromUpdate(CheckFiringInput);
            EntryPoint.UnsubscribeFromUpdate(CheckMousePositionInput);
            EntryPoint.UnsubscribeFromUpdate(CheckChangeWeaponInput);
        }

        private void CheckVerticalInput()
        {
            float verticalOffset = UnityEngine.Input.GetAxis(Vertical);
            float inputValue = CalculateInputValue(verticalOffset, verticalAxisInputMultiplier);
            OnVerticalInput(inputValue);
        }

        private void CheckFiringInput()
        {
            bool value = UnityEngine.Input.GetKey(PrimaryFire);
            OnPrimaryFireInput(value);
        }

        private void CheckMousePositionInput()
        {
            Vector3 value = UnityEngine.Input.mousePosition;
            OnMousePositionInput(value);
        }

        private void CheckChangeWeaponInput()
        {
            bool value = UnityEngine.Input.GetKeyDown(ChangeWeapon);
            OnChangeWeaponInput(value);
        }

        private static float CalculateInputValue(float axisOffset, float inputMultiplier)
            => axisOffset * inputMultiplier;
    }
}