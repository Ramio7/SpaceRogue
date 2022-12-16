using Abstracts;
using Gameplay;
using Gameplay.GameState;
using Scriptables;
using UI;
using UI.MainMenu;
using UnityEngine;
using Utilities.ResourceManagement;

public sealed class MainController : BaseController
{
    private readonly CurrentState _currentState;
    private readonly MainUIController _mainUIController;

    private readonly LevelProgressConfig _levelProgressConfig;
    private readonly ResourcePath _levelProgressConfigPath = new(Constants.Configs.LevelProgressConfig);

    private GameController _gameController;
    private MainMenuController _mainMenuController;
    

    public MainController(CurrentState currentState, Transform uiPosition)
    {
        _currentState = currentState;

        _mainUIController = new(uiPosition);
        AddController(_mainUIController);
        
        _levelProgressConfig = ResourceLoader.LoadObject<LevelProgressConfig>(_levelProgressConfigPath);
        
        _currentState.CurrentGameState.Subscribe(OnGameStateChange);
        OnGameStateChange(_currentState.CurrentGameState.Value);

    }

    protected override void OnDispose()
    {
        DisposeAllControllers();
        
        _currentState.CurrentGameState.Unsubscribe(OnGameStateChange);
        _levelProgressConfig.ResetCompletedLevels();
    }

    private void OnGameStateChange(GameState newState)
    {
        DisposeAllControllers();
        
        switch (newState)
        {
            case GameState.Menu:
                _mainMenuController = new MainMenuController(_currentState, _mainUIController.MainCanvas, _levelProgressConfig);
                break;
            case GameState.Game:
                _gameController = new GameController(_currentState, _mainUIController.MainCanvas, _levelProgressConfig);
                break;
            case GameState.None:
            default: break;
        }
    }

    private void DisposeAllControllers()
    {
        _gameController?.Dispose();
        _mainMenuController?.Dispose();
    }
    
}