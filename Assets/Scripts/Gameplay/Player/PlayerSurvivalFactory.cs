using System;
using Gameplay.Abstracts;
using Gameplay.Survival;
using Zenject;

namespace Gameplay.Player
{
    public sealed class PlayerSurvivalFactory : PlaceholderFactory<EntityView, EntitySurvival>
    {
        private readonly EntitySurvivalFactory _entitySurvivalFactory;
        private readonly EntitySurvivalConfig _playerSurvivalConfig;

        public event Action<EntitySurvival> PlayerSurvivalCreated = _ => { };

        public PlayerSurvivalFactory(EntitySurvivalFactory entitySurvivalFactory, EntitySurvivalConfig playerSurvivalConfig)
        {
            _entitySurvivalFactory = entitySurvivalFactory;
            _playerSurvivalConfig = playerSurvivalConfig;
        }

        public override EntitySurvival Create(EntityView view)
        {
            var playerSurvival = _entitySurvivalFactory.Create(view, _playerSurvivalConfig);
            PlayerSurvivalCreated.Invoke(playerSurvival);
            return playerSurvival;
        }
    }
}