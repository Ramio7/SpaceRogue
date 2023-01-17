using Abstracts;
using Gameplay.Enemy;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Scriptables;
using System;
using System.Collections.Generic;
using UI.Game;
using UnityEngine;
using Utilities.ResourceManagement;

namespace Gameplay.LevelProgress
{
    public sealed class LevelProgressController : BaseController
    {
        private readonly GameDataController _gameDataController;
        private readonly LevelProgressConfig _config;
        private readonly Timer _levelTimer;
        private readonly LevelTimerView _levelTimerView;
        private readonly LevelNumberView _levelNumberView;
        private readonly PlayerController _playerController;
        private readonly List<EnemyView> _enemyViews;
        private readonly EnemiesCountView _enemiesCountView;
        private int _enemiesCountToWin;
        
        private readonly ResourcePath _levelProgressConfigPath = new(Constants.Configs.LevelProgressConfig);

        public event Action<float> LevelComplete = _ => { };

        public LevelProgressController(GameDataController gameDataController, PlayerController playerController, List<EnemyView> enemyViews)
        {
            _gameDataController = gameDataController;
            _config = ResourceLoader.LoadObject<LevelProgressConfig>(_levelProgressConfigPath);
            _gameDataController.ResetPlayerHealthAndShield();
            _levelTimer = new(_config.LevelTimerInSeconds);
            _levelTimer.Start();
            
            _levelTimerView = GameUIController.LevelTimerView;
            _levelTimerView.Init(GetTimerText(_levelTimer.CurrentValue));

            _levelNumberView = GameUIController.LevelNumberView;
            _levelNumberView.InitNumber(_gameDataController.CompletedLevels + 1);

            _playerController = playerController;
            _playerController.PlayerDestroyed += StopExecute;
            _playerController.NextLevelInput.Subscribe(NextLevel);

            _enemyViews = enemyViews;
            _enemiesCountView = GameUIController.EnemiesCountView;
            _enemiesCountToWin = Mathf.Clamp(_config.EnemiesCountToWin, 1, _enemyViews.Count);
            _enemiesCountView.Init(0, _enemiesCountToWin);

            LevelComplete += OnLevelComplete;

            EntryPoint.SubscribeToUpdate(CheckProgress);
        }

        protected override void OnDispose()
        {
            StopExecute();
        }

        public void UpdatePlayerHealthAndShieldInfo(float health, float shield)
        {
            _gameDataController.SetPlayerCurrentHealth(health);
            _gameDataController.SetPlayerCurrentShield(shield);
        }

        private void OnLevelComplete(float levelNumber)
        {
            StopExecute();
            _playerController.ControllerDispose();
        }

        private void StopExecute()
        {
            LevelComplete -= OnLevelComplete;
            _playerController.PlayerDestroyed -= StopExecute;
            _playerController.NextLevelInput.Unsubscribe(NextLevel);
            EntryPoint.UnsubscribeFromUpdate(CheckProgress);
        }

        private void NextLevel(bool isNextLevel)
        {
            if (isNextLevel)
            {
                _gameDataController.AddCompletedLevels();
                _gameDataController.UpdateRecord();
                LevelComplete(_gameDataController.CompletedLevels);
            }
        }

        private void CheckProgress()
        {
            _levelTimerView.UpdateText(GetTimerText(_levelTimer.CurrentValue));

            if (_levelTimer.IsExpired)
            {
                _playerController.DestroyPlayer();
                return;
            }

            var countEnemyDestroyed = default(int);
            foreach (var view in _enemyViews)
            {
                if(view == null)
                {
                    countEnemyDestroyed++;
                }
            }
            _enemiesCountView.UpdateCounter(countEnemyDestroyed);

            if(countEnemyDestroyed >= _enemiesCountToWin)
            {
                _playerController.NextLevelInput.Value = true;
            }
        }

        private string GetTimerText(float time)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            return string.Format($"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
        }
    }
}