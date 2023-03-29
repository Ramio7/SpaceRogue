using System;
using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Movement;
using Zenject;

namespace Gameplay.Player
{
    public sealed class PlayerMovementFactory : PlaceholderFactory<PlayerView, IUnitMovementInput, UnitMovementModel, UnitMovement>
    {
        private readonly UnitMovementFactory _unitMovementFactory;

        public event Action<UnitMovement> PlayerMovementCreated = _ => { };

        private PlayerMovementFactory(UnitMovementFactory unitMovementFactory)
        {
            _unitMovementFactory = unitMovementFactory;
        }

        public override UnitMovement Create(PlayerView playerView, IUnitMovementInput movementInput, UnitMovementModel model)
        {
            var playerMovement = _unitMovementFactory.Create(playerView, movementInput, model);
            PlayerMovementCreated.Invoke(playerMovement);
            return playerMovement;
        }
    }
}