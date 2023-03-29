using Abstracts;
using Gameplay.Abstracts;

namespace Gameplay.Shooting
{
    public class UnmountedWeapon : MountedWeapon
    {
        public UnmountedWeapon(Weapon weapon, EntityView entityView) : base(weapon, entityView) { }
        
        public override void CommenceFiring()
        {
            //Does nothing, weapon is unmounted
        }
    }
}