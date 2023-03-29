using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Shooting.Factories;
using UnityEngine;

namespace Gameplay.Shooting
{
    public sealed class FrontalMountedWeapon : MountedWeapon
    {
        private readonly Transform _gunPointViewTransform;
        
        public FrontalMountedWeapon(Weapon weapon, EntityView entityView, GunPointViewFactory gunPointViewFactory) : base(weapon, entityView)
        {
            var unitScale = UnitViewTransform.localScale;
            var gunPointPosition = UnitViewTransform.position + UnitViewTransform.TransformDirection(0.6f * Mathf.Max(unitScale.x, unitScale.y) * Vector3.up);
            var gunPoint = gunPointViewFactory.Create(gunPointPosition, UnitViewTransform.rotation, UnitViewTransform);
            _gunPointViewTransform = gunPoint.transform;
        }

        public override void CommenceFiring()
        {
            Weapon.CommenceFiring(_gunPointViewTransform.position, _gunPointViewTransform.rotation);
        }
    }
}