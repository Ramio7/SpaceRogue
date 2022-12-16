using Abstracts;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Scriptables;
using System;
using UI.Game;
using Utilities.ResourceManagement;

namespace Gameplay.LevelProgress
{
    public sealed class LevelProgressController : BaseController
    {
        private readonly LevelProgressConfig _config;
        private readonly Timer _levelTimer;
        private readonly LevelTimerView _levelTimerView;
        private readonly PlayerController _playerController;

        private readonly ResourcePath _configPath = new(Constants.Configs.LevelProgressConfig);

        public LevelProgressController(PlayerController playerController)
        {
            _config = ResourceLoader.LoadObject<LevelProgressConfig>(_configPath);
            _levelTimer = new(_config.LevelTimerInSeconds);
            _levelTimer.Start();
            _levelTimerView = GameUIController.LevelTimerView;
            _levelTimerView.Init(GetTimerText(_levelTimer.CurrentValue));

            _playerController = playerController;
            _playerController.PlayerDestroyed += OnPlayerDestroyed;

            EntryPoint.SubscribeToUpdate(CheckProgress);
        }

        protected override void OnDispose()
        {
            _playerController.PlayerDestroyed -= OnPlayerDestroyed;
            EntryPoint.UnsubscribeFromUpdate(CheckProgress);
        }

        private void OnPlayerDestroyed()
        {
            _playerController.PlayerDestroyed -= OnPlayerDestroyed;
            EntryPoint.UnsubscribeFromUpdate(CheckProgress);
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