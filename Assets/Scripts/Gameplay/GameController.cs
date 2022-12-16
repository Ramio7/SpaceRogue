using Abstracts;
using Gameplay.Asteroid;
using Gameplay.Background;
using Gameplay.Camera;
using Gameplay.Enemy;
using Gameplay.GameEvent;
using Gameplay.GameState;
using Gameplay.LevelProgress;
using Gameplay.Player;
using Gameplay.Space;
using Scriptables;
using UI.Game;
using UnityEngine;
using Utilities.ResourceManagement;

namespace Gameplay
{
    public sealed class GameController : BaseController
    {
        private readonly CurrentState _currentState;
        private readonly GameUIController _gameUIController;
        private readonly AsteroidsController _asteroidsController;
        private readonly BackgroundController _backgroundController;
        private readonly SpaceController _spaceController;
        private readonly PlayerController _playerController;
        private readonly CameraController _cameraController;
        private readonly EnemyForcesController _enemyForcesController;
        private readonly GeneralGameEventsController _generalGameEventsController;
        private readonly LevelProgressController _levelProgressController;

        public GameController(CurrentState currentState, Canvas mainUICanvas, LevelProgressConfig levelProgressConfig)
        {
            _currentState = currentState;

            _gameUIController = new(mainUICanvas, ExitToMenu, NextLevel);
            AddController(_gameUIController);

            _playerController = new(levelProgressConfig.PlayerCurrentHealth, levelProgressConfig.PlayerCurrentShield);
            _backgroundController = new();
            AddController(_backgroundController);
            _spaceController = new();
            AddController(_spaceController);
            AddController(_playerController);
            _playerController.PlayerDestroyed += OnPlayerDestroyed;

            _cameraController = new(_playerController);
            AddController(_cameraController);

            _enemyForcesController = new(_playerController, _spaceController.GetEnemySpawnPoints());
            AddController(_enemyForcesController);

            _asteroidsController = new (_spaceController.GetEnemySpawnPoints());
            AddController(_asteroidsController);

            _generalGameEventsController = new(_playerController);
            AddController(_generalGameEventsController);
            _enemyForcesController = new(_playerController);
            AddController(_enemyForcesController);

            _levelProgressController = new(levelProgressConfig, _playerController);
            _levelProgressController.LevelComplete += LevelComplete;
            AddController(_levelProgressController);
        }

        private void OnPlayerDestroyed()
        {
            _gameUIController.AddDestroyPlayerMessage();
        }

        private void LevelComplete(float levelNumber)
        {
            _levelProgressController.UpdatePlayerHealthAndShieldInfo
                (_playerController.GetCurrentHealth(), _playerController.GetCurrentShield());
            _gameUIController.AddNextLevelMessage(levelNumber);
        }

        public void ExitToMenu() 
        {
            _currentState.CurrentGameState.Value = GameState.GameState.Menu;
        }

        public void NextLevel()
        {
            _currentState.CurrentGameState.Value = GameState.GameState.Game;
        }
    }
}