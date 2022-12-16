using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(LevelProgressConfig), menuName = "Configs/" + nameof(LevelProgressConfig))]
    public sealed class LevelProgressConfig : ScriptableObject
    {
        [field: SerializeField] public float LevelTimerInSeconds { get; private set; } = 60;

        public float CompletedLevels { get; private set; } = 0;
        public float RecordCompletedLevels { get; private set; } = 0;

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
            RecordCompletedLevels = CompletedLevels;
        }
    }
}