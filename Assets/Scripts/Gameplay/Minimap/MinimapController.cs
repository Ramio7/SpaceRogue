using Abstracts;
using Gameplay.Player;
using Scriptables;
using UI;
using UI.Game;
using UnityEngine;
using Utilities.ResourceManagement;

namespace Gameplay.Minimap
{
    public sealed class MinimapController : BaseController
    {
        private const int CameraZAxisOffset = -1;
        
        private readonly PlayerController _playerController;
        private readonly float _mapCameraSize;
        private readonly MinimapConfig _config;
        private readonly UnityEngine.Camera _minimapCamera;
        private readonly MinimapView _minimapView;

        private readonly ResourcePath _configPath = new(Constants.Configs.MinimapConfig);
        
        private readonly Transform _minimapCameraTransform;
        private readonly RectTransform _minimapRectTransform;
        private readonly Transform _playerTransform;

        private readonly float _anchoredPositionX;
        private readonly float _anchoredPositionY;

        private bool _isButtonPressed;

        public MinimapController(PlayerController playerController, float mapCameraSize)
        {
            _playerController = playerController;
            _mapCameraSize = mapCameraSize;

            _config = ResourceLoader.LoadObject<MinimapConfig>(_configPath);
            _minimapCamera = GameUIController.MinimapCamera;
            _minimapView = GameUIController.MinimapView;

            _minimapCameraTransform = _minimapCamera.transform;
            _minimapRectTransform = (RectTransform)_minimapView.transform;
            _anchoredPositionX = _minimapRectTransform.anchoredPosition.x;
            _anchoredPositionY = _minimapRectTransform.anchoredPosition.y;
            _playerTransform = _playerController.View.gameObject.transform;

            MinimapInit(_config.MinimapCameraSize, _config.MinimapColor, _config.MinimapAlpha);
            
            _playerController.MapInput.Subscribe(MapInput);
            EntryPoint.SubscribeToUpdate(FollowPlayer);
            _playerController.PlayerDestroyed += OnPlayerDestroyed;
        }

        private void MinimapInit(float cameraSize, Color color, float alpha)
        {
            _minimapCamera.orthographicSize = cameraSize;
            _minimapCamera.backgroundColor = color;
            _minimapView.SetColor(color);
            _minimapView.SetAlpha(alpha);
        }

        public void OnPlayerDestroyed()
        {
            _playerController.MapInput.Unsubscribe(MapInput);
            ReturnToMinimap();
            EntryPoint.UnsubscribeFromUpdate(FollowPlayer);
            _playerController.PlayerDestroyed -= OnPlayerDestroyed;
        }

        private void FollowPlayer()
        {
            if (_playerTransform == null || _isButtonPressed)
            {
                return;
            }

            var position = _playerTransform.position;
            _minimapCameraTransform.position = new(position.x, position.y, position.z + CameraZAxisOffset);
        }

        private void MapInput(bool mapInput)
        {
            if (mapInput & !_isButtonPressed)
            {
                _isButtonPressed = mapInput;

                EntryPoint.UnsubscribeFromUpdate(FollowPlayer);
                BecomeMap();
                return;
            }

            if (_isButtonPressed == mapInput)
            {
                return;
            }
            _isButtonPressed = mapInput;
            ReturnToMinimap();
            EntryPoint.SubscribeToUpdate(FollowPlayer);
        }

        private void BecomeMap()
        {
            _minimapCameraTransform.position = new(0, 0, CameraZAxisOffset);
            _minimapCamera.orthographicSize = _mapCameraSize;

            var mainRectTransform = (RectTransform)GameUIController.MainCanvas.transform;
            var newHeight = mainRectTransform.sizeDelta.y - _anchoredPositionY * 2;
            var newAnchoredPositionX = mainRectTransform.sizeDelta.x / 2 - newHeight / 2;
            _minimapRectTransform.sizeDelta = new(0, newHeight);
            _minimapRectTransform.anchoredPosition = new(newAnchoredPositionX, _anchoredPositionY);
        }

        private void ReturnToMinimap()
        {
            _minimapCamera.orthographicSize = _config.MinimapCameraSize;
            _minimapRectTransform.sizeDelta = new(0, _config.MinimapHeight);
            _minimapRectTransform.anchoredPosition = new(_anchoredPositionX, _anchoredPositionY);
        }
    }
}