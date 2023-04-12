using Gameplay.Space.Planets;
using Zenject;
using Gameplay.Space.SpaceObjects.Scriptables;
using UnityEngine; 

namespace Gameplay.Space.Factories
{
    public class PlanetMovementFactory : PlaceholderFactory<PlanetView, PlanetConfig, Transform, PlanetMovement>
    {
    }
}