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


        public ProjectileFactory(ProjectileConfig projectileConfig, ProjectileView view, 
            Transform projectileSpawnTransform, UnitType unitType)
        {
            _config = projectileConfig;
            _view = view;
            _projectileSpawnTransform = projectileSpawnTransform;
            _unitType = unitType;
        }

        public ProjectileController CreateProjectile() => CreateProjectile(Vector3.up);
        public ProjectileController CreateProjectile(Vector3 direction) => new(_config, CreateProjectileView(), _projectileSpawnTransform.parent.TransformDirection(direction), _unitType);

        private ProjectileView CreateProjectileView() => Object.Instantiate(_view, _projectileSpawnTransform.position, Quaternion.identity);
    }
}