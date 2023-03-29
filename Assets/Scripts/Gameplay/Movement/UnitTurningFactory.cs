using Abstracts;
using Gameplay.Abstracts;
using Zenject;

namespace Gameplay.Movement
{
    public sealed class UnitTurningFactory : PlaceholderFactory<EntityView, IUnitTurningInput, UnitMovementModel, UnitTurning>
    {
    }
}