using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(CompletedLevelsConfig), menuName = "Configs/" + nameof(CompletedLevelsConfig))]
    public sealed class CompletedLevelsConfig : ScriptableObject
    {
        [field: SerializeField] public float LevelTimerInSeconds { get; private set; } = 60;

        public float CompletedLevels { get; private set; } = 0;
        public float CompletedLevelsRecord { get; private set; } = 0;

        public void ResetCompletedLevels()
        {
            CompletedLevels = 0;
        }

        public void AddCompletedLevels()
        {
            CompletedLevels++;
        }

        public void UpdateRecord()
        {
            CompletedLevelsRecord = CompletedLevels;
        }
    }
}