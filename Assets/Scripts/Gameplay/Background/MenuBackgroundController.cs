using Abstracts;
using Scriptables;
using UnityEngine;
using Utilities.ResourceManagement;

namespace Gameplay.Background
{
    public sealed class MenuBackgroundController : BaseController
    {
        private readonly ResourcePath _configPath = new(Constants.Configs.Background.MenuBackgroundConfig);
        private readonly ResourcePath _viewPath = new(Constants.Prefabs.Stuff.MenuBackground);

        private readonly MenuBackgroundConfig _config;
        private readonly MenuBackgroundView _view;
        private readonly UnityEngine.Camera _camera;
        private readonly Transform _target;

        private readonly InfiniteSprite _backParalax;
        private readonly InfiniteSprite _midParalax;
        private readonly InfiniteSprite _foreParalax;

        private readonly NebulaEffect _nebulaEffect;

        public MenuBackgroundController()
        {
            _config = ResourceLoader.LoadObject<MenuBackgroundConfig>(_configPath);
            _view = LoadView<MenuBackgroundView>(_viewPath);
            _camera = UnityEngine.Camera.main;
            _camera.transform.position = new(0, 0, _camera.transform.position.z);
            _target = _camera.transform;

            _backParalax = new(_target, _view.BackSpriteRenderer, _config.BackCoefficient);
            _midParalax = new(_target, _view.MidSpriteRenderer, _config.MidCoefficient);
            _foreParalax = new(_target, _view.ForeSpriteRenderer, _config.ForeCoefficient);

            _nebulaEffect = new(_target, _view.NebulaParticleSystem.transform, _config.NebulaCoefficient);

            EntryPoint.SubscribeToLateUpdate(PlayAllEffects);
        }

        protected override void OnDispose()
        {
            EntryPoint.UnsubscribeFromLateUpdate(PlayAllEffects);
        }

        private void PlayAllEffects()
        {
            var mousePoint = _camera.ScreenToViewportPoint(UnityEngine.Input.mousePosition) * _config.CameraCoefficient;
            _target.position = new(mousePoint.x, mousePoint.y, _target.position.z);
            
            _backParalax.Play();
            _midParalax.Play();
            _foreParalax.Play();

            _nebulaEffect.Play();
        }
    }
}