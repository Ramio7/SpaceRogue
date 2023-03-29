using Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views;
using Object = UnityEngine.Object;


namespace Gameplay.Space.SpaceObjects.SpaceObjectsEffects
{
    public sealed class DamageOnTouchEffect : SpaceObjectEffect
    {
        private readonly DamageOnTouchEffectView _effectView;

        public DamageOnTouchEffect(DamageOnTouchEffectView effectView)
        {
            _effectView = effectView;
        }

        public override void Dispose()
        {
            Object.Destroy(_effectView.gameObject);
        }
    }
}
