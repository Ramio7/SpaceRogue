using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views;
using UnityEngine;
using Zenject;

namespace Gameplay.Space.Factories
{
    public class GravitationAuraViewFactory : PlaceholderFactory<Transform, GravitationAuraConfig,  GravitationAuraEffectView>
    {
        private readonly DiContainer _diContainer;

        public GravitationAuraViewFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public override GravitationAuraEffectView Create(Transform transform, GravitationAuraConfig config)
        {
            var view = _diContainer.InstantiatePrefabForComponent<GravitationAuraEffectView>(config.Prefab, transform.position, Quaternion.identity, transform);
            var size =  config.Radius;
            view.transform.localScale = new Vector3(size, size, 1);
            return view;
        }
    }
}