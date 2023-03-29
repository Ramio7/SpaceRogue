using Abstracts;
using Gameplay.Camera;
using System;
using Gameplay.Abstracts;
using UnityEngine;
using Utilities.Unity;

namespace Gameplay.Movement
{
    public sealed class UnitTurningMouse : IDisposable
    {
        private readonly UnityEngine.Camera _camera;
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly IUnitTurningMouseInput _turningMouseInput;
        private readonly UnitMovementModel _model;

        private float _lastTurnRate;

        public UnitTurningMouse(
            CameraView cameraView,
            EntityView entityView,
            IUnitTurningMouseInput turningMouseInput,
            UnitMovementModel model)
        {
            _camera = cameraView.GetComponent<UnityEngine.Camera>();
            _transform = entityView.transform;
            _rigidbody = entityView.GetComponent<Rigidbody2D>();
            _turningMouseInput = turningMouseInput;
            _model = model;

            _turningMouseInput.MousePositionInput += HandleHorizontalMouseInput;
        }

        public void Dispose()
        {
            _turningMouseInput.MousePositionInput -= HandleHorizontalMouseInput;
        }

        private void HandleHorizontalMouseInput(Vector3 newMousePositionInput)
        {
            var mousePosition = _camera.ScreenToWorldPoint(newMousePositionInput);
            mousePosition.z = 0;

            var direction = (mousePosition - _transform.position).normalized;
            var currentDirection = _transform.TransformDirection(Vector3.up);
            float angle = Vector2.SignedAngle(direction, currentDirection);

            Quaternion newRotation;
            if (UnityHelper.Approximately(angle, 0, Mathf.Abs(_lastTurnRate)))
            {
                _model.StopTurning();
                _lastTurnRate = _model.StartingTurnSpeed;

                newRotation = angle > 0
                    ? GetNewRotation(-angle, Vector3.forward)
                    : GetNewRotation(angle, Vector3.back);

                _rigidbody.MoveRotation(newRotation);
                _rigidbody.angularVelocity = 0;
                return;
            }

            if (angle > 0)
            {
                _model.Turn(true);
                newRotation = GetNewRotation(_model.CurrentTurnRate, Vector3.forward);
            }
            else
            {
                _model.Turn(false);
                newRotation = GetNewRotation(-_model.CurrentTurnRate, Vector3.back);
            }

            _lastTurnRate = _model.CurrentTurnRate;
            _rigidbody.MoveRotation(newRotation);
        }

        private Quaternion GetNewRotation(float angle, Vector3 axis)
        {
            return _transform.rotation * Quaternion.AngleAxis(angle, axis);
        }
    }
}