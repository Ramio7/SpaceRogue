using UnityEngine;

namespace Gameplay.Background
{
    public sealed class MenuBackgroundView : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer BackSpriteRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer MidSpriteRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer ForeSpriteRenderer { get; private set; }
        [field: SerializeField] public ParticleSystem NebulaParticleSystem { get; private set; }
    }
}