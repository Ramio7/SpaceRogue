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
using UI.Game;
using UnityEngine;

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

        public GameController(CurrentState currentState, Canvas mainUICanvas)
        {
            _currentState = currentState;

            _gameUIController = new(mainUICanvas, ExitToMenu);
            AddController(_gameUIController);

            _backgroundController = new();
            AddController(_backgroundController);

            _spaceController = new();
            AddController(_spaceController);

            _playerController = new(_spaceController.GetPlayerSpawnPoint());
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

            _levelProgressController = new(_playerController);
            AddController(_levelProgressController);
        }

        private void OnPlayerDestroyed()
        {
            _gameUIController.AddDestroyPlayerMessage();
        }

        public void ExitToMenu() 
        {
            _currentState.CurrentGameState.Value = GameState.GameState.Menu;
        }
    }
}