using System;
using System.Collections.Generic;
using Gameplay.Space.SpaceObjects.SpaceObjectsEffects;
using Object = UnityEngine.Object;

namespace Gameplay.Space.SpaceObjects
{
    public class SpaceObject : IDisposable
    {
        private readonly SpaceObjectView _view;
        private readonly List<SpaceObjectEffect> _objectEffects;

        public SpaceObject(SpaceObjectView view, List<SpaceObjectEffect> objectEffects)
        {
            _view = view;
            _objectEffects = objectEffects;
        }

        public void Dispose()
        {
            DisposeObjectEffects();
            Object.Destroy(_view.gameObject);
        }

        private void DisposeObjectEffects()
        {
            foreach (var effect in _objectEffects)
            {
                effect.Dispose();
            }
            _objectEffects.Clear();
        }
    }
}