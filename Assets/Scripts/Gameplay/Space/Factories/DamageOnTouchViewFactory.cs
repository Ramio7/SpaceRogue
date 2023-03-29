using Gameplay.Space.SpaceObjects.Scriptables;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views;
using UnityEngine;
using Zenject;
using Gameplay.Damage;

namespace Gameplay.Space.Factories
{
    public class DamageOnTouchViewFactory : PlaceholderFactory<Transform, DamageOnTouchConfig, DamageOnTouchEffectView>
    {
        private readonly DiContainer _diContainer;

        public DamageOnTouchViewFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public override DamageOnTouchEffectView Create(Transform transform, DamageOnTouchConfig config)
        {
            var view = _diContainer.InstantiatePrefabForComponent<DamageOnTouchEffectView>(config.Prefab, transform.position, Quaternion.identity, transform);
            var size = config.Radius;
            view.transform.localScale = new Vector3(size, size, 1);
            view.Init(new DamageModel(config.Damage));
            return view;
        }
    }
}