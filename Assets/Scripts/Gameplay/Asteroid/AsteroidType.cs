namespace Asteroid
{
    public enum AsteroidType
    {
        None = 0,
        Standart = 1,
        Fast = 2
    }
    public enum AsteroidMoveType
    {
        None = 0,
        Static = 1,
        OrbitalMotion = 2,
        LinearMotion = 3,
        PlayerTargeting = 4
    }

    public enum AsteroidCloudType
    {
        None = 0,
        SingleAsteroid = 1,
        SmallAsteroidCloud = 2,
        BigAsteroidCloud = 3
    }

    public enum AsteroidSizeType
    { 
        Small = 0,
        Middle = 1,
        Big = 2
    }
}