using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(LevelProgressConfig), menuName = "Configs/" + nameof(LevelProgressConfig))]
    public sealed class LevelProgressConfig : ScriptableObject
    {
        [field: SerializeField] public float LevelTimerInSeconds { get; private set; } = 60;
        [field: SerializeField] public int EnemiesCountToWin { get; private set; } = 10;
    }
}