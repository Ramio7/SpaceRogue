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


        public Asteroid(AsteroidView view, EntitySurvival survival)
        {
            _survival = survival;
            _view = view;

            _survival.UnitDestroyed += Dispose;
        }

        public void Dispose()
        {
            _survival.UnitDestroyed -= Dispose;
            _survival.Dispose();

            Object.Destroy(_view.gameObject);
        }

        public void StartRandomDirectedMovement(float startingSpeed)
        {
            var rigidbody = _view.GetComponent<Rigidbody2D>();
            var direction = RandomPicker.PickRandomAngle(0, 360);
            rigidbody.AddForce(direction * startingSpeed, ForceMode2D.Impulse);
        }

        public void StartTargetedMovement(float startingSpeed, float deviationRadius, Vector2 targetPosition)
        {
            var deviationDirection = Random.insideUnitCircle * deviationRadius;
            var baseXCoordinate = targetPosition.x;
            var baseYCoordinate = targetPosition.y;
            var target = new Vector2(baseXCoordinate + deviationDirection.x, baseYCoordinate + deviationDirection.y);
            var direction = (Vector2)_view.transform.position - target;
            var rigidbody = _view.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(direction * startingSpeed, ForceMode2D.Impulse);
        }

        public void StartEscapingMovement(float startingSpeed, Vector2 targetPosition)
        {
            var direction = targetPosition - (Vector2)_view.transform.position;
            var rigidbody = _view.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(direction * startingSpeed, ForceMode2D.Impulse);
        }
    }
}