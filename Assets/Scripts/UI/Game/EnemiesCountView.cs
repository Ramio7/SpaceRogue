using UI.Common;
using UnityEngine;

namespace UI.Game
{
    public sealed class EnemiesCountView : MonoBehaviour
    {
        [field: SerializeField] public TextView EnemiesDestroyedCount { get; private set; }
        [field: SerializeField] public TextView EnemiesCount { get; private set; }

        public void Init(float enemiesDestroyedCount, float enemiesCount)
        {
            EnemiesDestroyedCount.Init(enemiesDestroyedCount.ToString());
            EnemiesCount.Init(enemiesCount.ToString());
        }

        public void UpdateCounter(float enemiesDestroyedCount)
        {
            EnemiesDestroyedCount.UpdateText(enemiesDestroyedCount.ToString());
        }
    }
}