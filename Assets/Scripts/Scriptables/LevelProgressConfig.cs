using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(LevelProgressConfig), menuName = "Configs/" + nameof(LevelProgressConfig))]
    public sealed class LevelProgressConfig : ScriptableObject
    {
        [field: SerializeField] public float LevelTimerInSeconds { get; private set; } = 60;

        public float CompletedLevels { get; private set; } = 0;
        public float RecordCompletedLevels { get; private set; } = 0;
        public float PlayerCurrentHealth { get; private set; } = 0;
        public float PlayerCurrentShield { get; private set; } = 0;

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
            if (RecordCompletedLevels < CompletedLevels)
            {
                RecordCompletedLevels = CompletedLevels; 
            }
        }

        public void ResetRecord()
        {
            RecordCompletedLevels = 0;
        }

        public void SetPlayerCurrentHealth(float health)
        {
            PlayerCurrentHealth = health;
        }

        public void SetPlayerCurrentShield(float shield)
        {
            PlayerCurrentShield = shield;
        }

        public void ResetPlayerHealthAndShield()
        {
            PlayerCurrentHealth = 0;
            PlayerCurrentShield = 0;
        }
    }
}