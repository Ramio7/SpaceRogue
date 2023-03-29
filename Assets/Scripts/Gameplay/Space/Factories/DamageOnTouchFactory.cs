using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using UnityEngine;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class DamageOnTouchFactory : PlaceholderFactory<Transform, DamageOnTouchConfig, DamageOnTouchEffect>
    {
        private readonly DamageOnTouchViewFactory _viewFactory;

        public DamageOnTouchFactory(DamageOnTouchViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public override DamageOnTouchEffect Create(Transform transform, DamageOnTouchConfig config)
        {
            var view = _viewFactory.Create(transform, config);
            return new DamageOnTouchEffect(view);
        }
    }
}
