using Abstracts;
using UnityEngine;

namespace Gameplay.Shooting
{
    public sealed class ProjectileFactory
    {
        private readonly ProjectileConfig _config;
        private readonly ProjectileView _view;
        
        private readonly Transform _projectileSpawnTransform;
        private readonly UnitType _unitType;
        private readonly GameObject _projectilePool;


        public ProjectileFactory(ProjectileConfig projectileConfig, ProjectileView view, 
            Transform projectileSpawnTransform, UnitType unitType)
        {
            _config = projectileConfig;
            _view = view;
            _projectileSpawnTransform = projectileSpawnTransform;
            _unitType = unitType;

            _projectilePool = GameObject.Find("ProjectilePool");
            if (_projectilePool == null)
            {
                _projectilePool = new GameObject("ProjectilePool");
                _projectilePool.transform.position.Set(9999, 9999, 0);
            }
        }

        public ProjectileController CreateProjectile() => CreateProjectile(Vector3.up);
        public ProjectileController CreateProjectile(Vector3 direction) => new(_config, CreateProjectileView(), _projectileSpawnTransform.parent.TransformDirection(direction), _unitType);

        private ProjectileView CreateProjectileView()
        {
            var projectile = Object.Instantiate(_view, _projectileSpawnTransform.position, Quaternion.identity);
            projectile.transform.SetParent(_projectilePool.transform, true);

            return projectile;
        }
    }
}