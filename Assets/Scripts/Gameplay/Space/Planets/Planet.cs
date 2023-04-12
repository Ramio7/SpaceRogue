using System;
using Object = UnityEngine.Object;

namespace Gameplay.Space.Planets
{
    public class Planet : IDisposable
    {
        private readonly PlanetView _planetView;
        private readonly PlanetMovement _planetMovement;

        public event Action<Planet> PlanetDestroyed = (_) => { };

        public Planet(PlanetView planetView, PlanetMovement planetMovement)
        {
            _planetView = planetView;
            _planetMovement = planetMovement;

            _planetView.CollidedSpaceObject += OnSpaceObjectCollision;
            _planetView.CollidedPlanet += OnPlanetCollision;
        }

        public void Dispose()
        {
            PlanetDestroyed.Invoke(this);
            
            _planetView.CollidedSpaceObject -= OnSpaceObjectCollision;
            _planetView.CollidedPlanet -= OnPlanetCollision;
            
            _planetMovement.Dispose();

            Object.Destroy(_planetView.gameObject);
        }
        
        private void OnSpaceObjectCollision()
        {
            Dispose();
        }

        private void OnPlanetCollision()
        {
            //TODO implement asteroid cloud when ready
            Dispose();
        }
    }
}