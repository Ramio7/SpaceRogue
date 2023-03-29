using Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views;
using Object = UnityEngine.Object;
using Gameplay.Mechanics.Timer;
using Gameplay.Damage;

namespace Gameplay.Space.SpaceObjects.SpaceObjectsEffects
{
    public sealed class DamageAuraEffect : SpaceObjectEffect
    {
        private readonly DamageAuraView _view;
        private readonly Timer _cooldownTimer;

        public DamageAuraEffect(DamageModel damageModel, DamageAuraView view, Timer cooldownTimer)
        {
            _cooldownTimer = cooldownTimer;
            _view = view;
            _view.Init(damageModel, _cooldownTimer);
        }

        public override void Dispose()
        {
            _cooldownTimer.Dispose();
            Object.Destroy(_view.gameObject);
        }
    }
}