using System;
using Gameplay.Survival;
using UnityEngine;
using Utilities.Mathematics;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Gameplay.Asteroids
{
    public class Asteroid : IDisposable
    {
        private readonly EntitySurvival _survival;
        private readonly AsteroidView _view;
        private readonly Rigidbody2D _rigidbody;

        public event Action<Asteroid> AsteroidDestroyed = _ => { };
        public string Id { get; private set; }

        public Asteroid(AsteroidView view, EntitySurvival survival)
        {
            Id = view.GetHashCode().ToString();
            _survival = survival;
            _view = view;
            _rigidbody = _view.GetComponent<Rigidbody2D>();

            _survival.UnitDestroyed += Dispose;
        }

        public void Dispose()
        {
            _survival.UnitDestroyed -= Dispose;
            _survival.Dispose();

            Object.Destroy(_view.gameObject);

            AsteroidDestroyed?.Invoke(this);
        }

        public void StartRandomDirectedMovement(float startingSpeed) => 
            _rigidbody.AddForce(CalculateRandomDirection() * startingSpeed, ForceMode2D.Impulse);

        public void StartTargetedMovement(float startingSpeed, float deviationRadius, Vector2 targetPosition) => 
            _rigidbody.AddForce(CalculateTargetedDirection(deviationRadius, targetPosition) * startingSpeed, ForceMode2D.Impulse);

        public void StartEscapingMovement(float startingSpeed, Vector2 targetPosition) => 
            _rigidbody.AddForce(CalculateEscapingDirection(targetPosition) * startingSpeed, ForceMode2D.Impulse);

        private static Vector3 CalculateRandomDirection() => RandomPicker.PickRandomAngle(0, 360).normalized;

        private Vector2 CalculateTargetedDirection(float deviationRadius, Vector2 targetPosition)
        {
            var deviationDirection = Random.insideUnitCircle * deviationRadius;
            var baseXCoordinate = targetPosition.x;
            var baseYCoordinate = targetPosition.y;
            var target = new Vector2(baseXCoordinate + deviationDirection.x, baseYCoordinate + deviationDirection.y);
            var direction = target - (Vector2)_view.transform.position;
            return direction.normalized;
        }

        private Vector2 CalculateEscapingDirection(Vector2 targetPosition) => ((Vector2)_view.transform.position - targetPosition).normalized;
    }
}