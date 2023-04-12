using Gameplay.Space.Planets;
using Gameplay.Space.SpaceObjects.Scriptables;
using UnityEngine;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class PlanetFactory : PlaceholderFactory<Vector2, PlanetConfig, Transform, Planet>
    {
        private readonly PlanetViewFactory _planetViewFactory;
        private readonly PlanetMovementFactory _planetMovementFactory;

        public PlanetFactory(PlanetViewFactory planetViewFactory, PlanetMovementFactory planetMovementFactory)
        {
            _planetViewFactory = planetViewFactory;
            _planetMovementFactory = planetMovementFactory;
        }
        public override Planet Create(Vector2 position, PlanetConfig config, Transform spaceObjectTransform)
        {
            var view = _planetViewFactory.Create(position, config);
            var movement = _planetMovementFactory.Create(view, config, spaceObjectTransform);
            return new Planet(view, movement);
        }
    }
}