using Abstracts;
using System;
using UnityEngine;
using Utilities.ResourceManagement;

namespace UI.Game
{
    public sealed class GameUIController : BaseController
    {
        public static PlayerStatusBarView PlayerStatusBarView { get; private set; }
        public static PlayerSpeedometerView PlayerSpeedometerView { get; private set; }
        public static PlayerWeaponView PlayerWeaponView { get; private set; }
        public static Transform EnemyHealthBars { get; private set; }
        public static Transform GameEventIndicators { get; private set; }

        private GameCanvasView _gameCanvasView;
        private DestroyPlayerMessageView _playerDestroyedMessageView;

        private readonly ResourcePath _gameCanvasPath = new(Constants.Prefabs.Canvas.Game.GameCanvas);
        private readonly ResourcePath _playerStatusBarCanvasPath = new(Constants.Prefabs.Canvas.Game.StatusBarCanvas);
        private readonly ResourcePath _playerSpeedometerCanvasPath = new(Constants.Prefabs.Canvas.Game.SpeedometerCanvas);
        private readonly ResourcePath _playerWeaponCanvasPath = new(Constants.Prefabs.Canvas.Game.WeaponCanvas);
        private readonly ResourcePath _playerDestroyedCanvasPath = new(Constants.Prefabs.Canvas.Game.DestroyPlayerCanvas);

        private readonly Action _exitToMenu;

        public GameUIController(Canvas mainCanvas, Action exitToMenu)
        {
            AddGameCanvas(mainCanvas.transform);
            _exitToMenu = exitToMenu;

            EnemyHealthBars = _gameCanvasView.EnemyHealthBars;

            AddPlayerStatusBar();
            AddPlayerSpeedometer();
            AddPlayerWeapon();
        }

        private void AddGameCanvas(Transform transform)
        {
            _gameCanvasView = ResourceLoader.LoadPrefabAsChild<GameCanvasView>(_gameCanvasPath, transform);
            AddGameObject(_gameCanvasView.gameObject);
        }

        private void AddPlayerStatusBar()
        {
            PlayerStatusBarView = ResourceLoader.LoadPrefabAsChild<PlayerStatusBarView>
                (_playerStatusBarCanvasPath, _gameCanvasView.PlayerInfo);
            AddGameObject(PlayerStatusBarView.gameObject);
        }

        private void AddPlayerSpeedometer()
        {
            PlayerSpeedometerView = ResourceLoader.LoadPrefabAsChild<PlayerSpeedometerView>
                (_playerSpeedometerCanvasPath, _gameCanvasView.PlayerInfo);
            AddGameObject(PlayerSpeedometerView.gameObject);
        }

        private void AddPlayerWeapon()
        {
            PlayerWeaponView = ResourceLoader.LoadPrefabAsChild<PlayerWeaponView>
                (_playerWeaponCanvasPath, _gameCanvasView.PlayerInfo);
            AddGameObject(PlayerWeaponView.gameObject);
        }

        protected override void OnDispose()
        {
            PlayerStatusBarView = null;
            PlayerSpeedometerView = null;
            PlayerWeaponView = null;
            EnemyHealthBars = null;
            GameEventIndicators = null;
        }
            
        public void AddDestroyPlayerMessage()
        {
            _playerDestroyedMessageView = ResourceLoader.LoadPrefabAsChild<DestroyPlayerMessageView>
                (_playerDestroyedCanvasPath, _gameCanvasView.transform);
            _playerDestroyedMessageView.Init(_exitToMenu);
            AddGameObject(_playerDestroyedMessageView.gameObject);
        }
    }
}