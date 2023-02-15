using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = nameof(LevelProgressConfig), menuName = "Configs/" + nameof(LevelProgressConfig))]
    public sealed class LevelProgressConfig : ScriptableObject
    {
        [field: SerializeField] public int EnemiesCountToWin { get; private set; } = 10;
    }
}