using Abstracts;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Scriptables;
using System;
using UI.Game;

namespace Gameplay.LevelProgress
{
    public sealed class LevelProgressController : BaseController
    {
        private readonly LevelProgressConfig _config;
        private readonly Timer _levelTimer;
        private readonly LevelTimerView _levelTimerView;
        private readonly LevelNumberView _levelNumberView;
        private readonly PlayerController _playerController;

        public event Action<float> LevelComplete = _ => { };

        public LevelProgressController(LevelProgressConfig levelProgressConfig, PlayerController playerController)
        {
            _config = levelProgressConfig;
            _config.ResetPlayerHealthAndShield();
            _levelTimer = new(_config.LevelTimerInSeconds);
            _levelTimer.Start();
            
            _levelTimerView = GameUIController.LevelTimerView;
            _levelTimerView.Init(GetTimerText(_levelTimer.CurrentValue));

            _levelNumberView = GameUIController.LevelNumberView;
            _levelNumberView.InitNumber(_config.CompletedLevels + 1);

            _playerController = playerController;
            _playerController.PlayerDestroyed += StopExecute;
            _playerController.NextLevelInput.Subscribe(NextLevel);

            LevelComplete += OnLevelComplete;

            EntryPoint.SubscribeToUpdate(CheckProgress);
        }

        protected override void OnDispose()
        {
            StopExecute();
        }

        public void UpdatePlayerHealthAndShieldInfo(float health, float shield)
        {
            _config.SetPlayerCurrentHealth(health);
            _config.SetPlayerCurrentShield(shield);
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
                _config.AddCompletedLevels();
                _config.UpdateRecord();
                LevelComplete(_config.CompletedLevels);
            }
        }

        private void CheckProgress()
        {
            _levelTimerView.UpdateText(GetTimerText(_levelTimer.CurrentValue));

            if (_levelTimer.IsExpired)
            {
                _playerController.DestroyPlayer();
            }
        }

        private string GetTimerText(float time)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            return string.Format($"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
        }
    }
}