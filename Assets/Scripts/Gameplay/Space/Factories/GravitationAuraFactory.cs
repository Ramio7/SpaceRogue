using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using UnityEngine;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class GravitationAuraFactory : PlaceholderFactory<Transform, GravitationAuraConfig, GravitationAuraEffect>
    {
        private readonly GravitationAuraViewFactory _viewFactory;

        public GravitationAuraFactory(GravitationAuraViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public override GravitationAuraEffect Create(Transform transform, GravitationAuraConfig config)
        {
            var view = _viewFactory.Create(transform, config);
            return new GravitationAuraEffect(view, config);
        }
    }
}
