using Abstracts;
using System;
using Gameplay.Abstracts;
using UnityEngine;

namespace Gameplay.Movement
{
    public sealed class UnitTurning : IDisposable
    {
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly IUnitTurningInput _turningInput;
        private readonly UnitMovementModel _model;

        public UnitTurning(
            EntityView entityView,
            IUnitTurningInput turningInput,
            UnitMovementModel model)
        {
            _transform = entityView.transform;
            _rigidbody = entityView.GetComponent<Rigidbody2D>();
            _turningInput = turningInput;
            _model = model;

            _turningInput.HorizontalAxisInput += HandleHorizontalInput;
        }

        public void Dispose()
        {
            _turningInput.HorizontalAxisInput -= HandleHorizontalInput;
        }

        private void HandleHorizontalInput(float newInputValue)
        {
            Quaternion newRotation = Quaternion.identity;
            switch (newInputValue)
            {
                case 0:
                    _model.StopTurning();
                    newRotation = _transform.rotation;
                    break;
                case < 0:
                    _model.Turn(true);
                    newRotation = _transform.rotation * Quaternion.AngleAxis(_model.CurrentTurnRate * newInputValue, Vector3.forward);
                    break;
                case > 0:
                    _model.Turn(false);
                    newRotation = _transform.rotation * Quaternion.AngleAxis(_model.CurrentTurnRate * newInputValue, Vector3.back);
                    break;
            }
            _rigidbody.MoveRotation(newRotation);
        }
    }
}