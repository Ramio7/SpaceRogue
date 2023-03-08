using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Utilities.Mathematics;

public class AsteroidRandomDirectionMotionBehaviour : AsteroidLinearMotionBehaviorBase
{
    public AsteroidRandomDirectionMotionBehaviour(AsteroidView view, AsteroidBehaviourConfig config) : base(view, config)
    {
        AsteroidStart();
    }

    protected override void AsteroidStart()
    {
        AsteroidDirection = RandomPicker.PickRandomAngle(0, 360, new());
        Move(AsteroidDirection, Config.AsteroidStartingForce);
    }
}
