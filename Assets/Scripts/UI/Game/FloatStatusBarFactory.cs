using Zenject;
using Gameplay.Survival;
using Gameplay.Camera;
using Services;
using UnityEngine;

namespace UI.Game
{
    public sealed class FloatStatusBarFactory : PlaceholderFactory<HealthStatusBarView, Collider2D, EntitySurvival, FloatStatusBar>
    {
        private readonly Updater _updater;
        private readonly CameraView _cameraView;
        private readonly float _scaleFactor;

        public FloatStatusBarFactory(Updater updater, CameraView cameraView, EnemyHealthBarsView barsView)
        {
            _updater = updater;
            _cameraView = cameraView;
            _scaleFactor = barsView.GetComponentInParent<Canvas>().scaleFactor;
        }

        public override FloatStatusBar Create(HealthStatusBarView statusBarView, Collider2D unitCollider, EntitySurvival entitySurvival)
        {
            return new FloatStatusBar(_updater, _cameraView, _scaleFactor, statusBarView, unitCollider, entitySurvival);
        }
    }
}