using Abstracts;
using Gameplay.Abstracts;

namespace Gameplay.Player
{
    public sealed class PlayerView : EntityView
    {
        public override EntityType EntityType => EntityType.Player;
    }
}