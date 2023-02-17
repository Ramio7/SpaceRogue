namespace Asteroid
{
    public enum AsteroidType
    {
        None = 0,
        OrdinaryAsteroid = 1,
        FastAsteroid = 2
    }
    public enum AsteroidMoveType
    {
        None = 0,
        Static = 1,
        OrbitalMotion = 2,
        LinearMotion = 3,
        PlayerTargeting = 4,
        CollisionEscaping = 5,
        CreatorEscaping = 6
    }

    public enum AsteroidCloudType
    {
        None = 0,
        SmallAsteroidCloud = 1,
        MediumAsteroidCloud = 2,
        BigAsteroidCloud = 3
    }

    public enum AsteroidCloudBehaviour
    {
        None = 0,
        Static = 1,
        CreatorEscaping = 2,
        CollisionEscaping = 3
    }

    public enum AsteroidSizeType
    { 
        None = 0,
        Small = 1,
        Medium = 2,
        Big = 3
    }

    public enum AsteroidConfigType
    {
        None = 0,
        SingleAsteroidConfig = 1,
        AstreoidCloudConfig = 2
    }
}