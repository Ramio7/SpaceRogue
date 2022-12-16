using Abstracts;
using Gameplay.Background;
using Gameplay.GameState;
using Scriptables;
using UI.Game;
using UnityEngine;
using Utilities.ResourceManagement;

namespace UI.MainMenu
{
    public sealed class MainMenuController : BaseController
    {
        private readonly CurrentState _currentState;
        private readonly CompletedLevelsConfig _completedLevelsConfig;
        
        private readonly ResourcePath _mainMenuCanvasPath = new(Constants.Prefabs.Canvas.Menu.MainMenuCanvas);
        private readonly ResourcePath _completedLevelsConfigPath = new(Constants.Configs.CompletedLevelsConfig);
        
        private MainMenuCanvasView _mainMenuCanvasView;

        public MainMenuController(CurrentState currentState, Canvas mainUICanvas)
        {
            _currentState = currentState;
            _completedLevelsConfig = ResourceLoader.LoadObject<CompletedLevelsConfig>(_completedLevelsConfigPath);
            _completedLevelsConfig.ResetCompletedLevels();
            AddMainMenuCanvas(mainUICanvas.transform);


            var menuBackgroundController = new MenuBackgroundController();
            AddController(menuBackgroundController);
        }

        private void AddMainMenuCanvas(Transform transform)
        {
            _mainMenuCanvasView = ResourceLoader.LoadPrefabAsChild<MainMenuCanvasView>(_mainMenuCanvasPath, transform);
            _mainMenuCanvasView.Init(StartGame, ExitGame, 
                _completedLevelsConfig.CompletedLevels, _completedLevelsConfig.CompletedLevelsRecord);
            AddGameObject(_mainMenuCanvasView.gameObject);
        }

        private void StartGame()
        {
            _currentState.CurrentGameState.Value = GameState.Game;
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}