using Abstracts;
using Gameplay.Asteroid;
using Gameplay.Damage;
using Gameplay.Space.Star;
using Scriptables.Space;
using UnityEngine;

namespace Gameplay.Space.Planet
{
    public sealed class PlanetController : BaseController
    {
        private readonly PlanetView _view;
        private readonly float _currentSpeed;
        private readonly bool _isMovingRetrograde;

        private readonly PlanetConfig _planetConfig;
        
        private readonly StarView _starView;

        public PlanetController(PlanetView view, StarView starView, float speed, bool isMovingRetrograde, float planetDamage, PlanetConfig config)
        {
            _planetConfig= config;
            
            _view = view;
            _view.transform.parent = starView.transform;

            var damageModel = new DamageModel(planetDamage);
            view.Init(damageModel);

            AddGameObject(view.gameObject);
            _starView = starView;
            _currentSpeed = speed;
            _isMovingRetrograde = isMovingRetrograde;
            _view.CollisionEnter += Dispose;
            _view.OnBigObjectCollision += CreateAsteroidCloudOnDestroy;

            EntryPoint.SubscribeToUpdate(Move);
        }

        protected override void OnDispose()
        {
            _view.OnBigObjectCollision -= CreateAsteroidCloudOnDestroy;
            _view.CollisionEnter -= Dispose;
            EntryPoint.UnsubscribeFromUpdate(Move);
        }

        private void CreateAsteroidCloudOnDestroy(Collision2D collision)
        {
            var asteroidFactory = new AsteroidFactory(_view);
            asteroidFactory.CreateAsteroidCloud(_view.transform.position, _planetConfig.AsteroidCloudConfig, collision);
            asteroidFactory.Dispose();
        }

        private void Move(float deltaTime)
        {
            if (_starView is not null)
            {
                _view.transform.RotateAround(
                    _starView.transform.position,
                    _isMovingRetrograde ? Vector3.forward : Vector3.back,
                    _currentSpeed * deltaTime
                );
            }
        }
    }
}