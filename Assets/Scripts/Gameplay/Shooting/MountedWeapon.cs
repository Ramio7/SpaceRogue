using Abstracts;
using Gameplay.Abstracts;
using UnityEngine;

namespace Gameplay.Shooting
{
    public abstract class MountedWeapon
    {
        protected Weapon Weapon { get; set; }
        protected Transform UnitViewTransform { get; set; }
        
        public MountedWeapon(Weapon weapon, EntityView entityView)
        {
            Weapon = weapon;
            UnitViewTransform = entityView.transform;
        }

        public abstract void CommenceFiring();
    }
}