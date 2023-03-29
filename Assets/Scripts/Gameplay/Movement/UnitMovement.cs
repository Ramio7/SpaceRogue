using Abstracts;
using System;
using Gameplay.Abstracts;
using UnityEngine;

namespace Gameplay.Movement
{
    public sealed class UnitMovement : IDisposable
    {
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly IUnitMovementInput _movementInput;
        private readonly UnitMovementModel _model;

        public float CurrentSpeed => _model.CurrentSpeed;
        public float MaxSpeed => _model.MaxSpeed;

        public UnitMovement(
            EntityView entityView,
            IUnitMovementInput movementInput,
            UnitMovementModel model)
        {
            _rigidbody = entityView.GetComponent<Rigidbody2D>();
            _transform = entityView.transform;
            _movementInput = movementInput;
            _model = model;

            _movementInput.VerticalAxisInput += HandleVerticalInput;
        }

        public void Dispose()
        {
            _movementInput.VerticalAxisInput -= HandleVerticalInput;
        }

        private void HandleVerticalInput(float newInputValue)
        {
            bool isZeroInput = Mathf.Approximately(newInputValue, 0);

            if (!isZeroInput)
            {
                _model.Accelerate(newInputValue > 0);
            }
            
            if (!Mathf.Approximately(CurrentSpeed, 0))
            {
                var forwardDirection = _transform.TransformDirection(Vector3.up);
                _rigidbody.AddForce(forwardDirection.normalized * CurrentSpeed, ForceMode2D.Force);
            }
            
            if (_rigidbody.velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
            {
                Vector3 velocity = _rigidbody.velocity.normalized * MaxSpeed;
                _rigidbody.velocity = velocity;
            }

            if (isZeroInput && CurrentSpeed < _model.StoppingSpeed && CurrentSpeed > -_model.StoppingSpeed)
            {
                _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, Time.deltaTime);
                _model.StopMoving();
            }
        }
    }
}