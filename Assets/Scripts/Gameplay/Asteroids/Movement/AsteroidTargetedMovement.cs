using Gameplay.Asteroids;
using Gameplay.Asteroids.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidTargetedMovement : IAsteroidMovementBehaviour
{
    private readonly AsteroidView _view;
    private readonly float _startingSpeed;
    private readonly Vector2 _targetPosition;
    private readonly float _directionScattering;

    public AsteroidTargetedMovement(AsteroidView view, float startingSpeed, Vector2 targetPosition, float directionScattering)
    {
        _view = view;
        _startingSpeed = startingSpeed;
        _targetPosition = targetPosition;
        _directionScattering = directionScattering;
    }

    public void StartMovement()
    {
        var directionDeviation = Random.insideUnitCircle * _directionScattering;
        var baseXCoordinate = _targetPosition.x;
        var baseYCoordinate = _targetPosition.y;
        var target = new Vector2(baseXCoordinate + directionDeviation.x, baseYCoordinate + directionDeviation.y);
        var direction = (Vector2)_view.transform.position - target;
        var rigidbody = _view.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(direction * _startingSpeed, ForceMode2D.Impulse);
    }
}
