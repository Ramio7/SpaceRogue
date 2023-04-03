using Gameplay.Asteroids.Movement;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class AsteroidRandomDirectedMovementFactory : PlaceholderFactory<float, AsteroidView, AsteroidRandomDirectedMovement>
    {
        public override AsteroidRandomDirectedMovement Create(float startingSpeed, AsteroidView view)
        {
            var movement = new AsteroidRandomDirectedMovement(view, startingSpeed);
            movement.StartMovement();
            return movement;
        }
    }
}
