using Gameplay.Mechanics.Timer;

namespace Gameplay.Asteroid.Behaviour
{
    public abstract class AsteroidBehaviour
    {
        protected readonly AsteroidView View;
        protected readonly AsteroidBehaviourConfig Config;

        protected Timer Timer;

        public void Dispose()
        {
            OnDispose();
            EntryPoint.UnsubscribeFromUpdate(OnUpdate);
        }

        protected AsteroidBehaviour(AsteroidView view, AsteroidBehaviourConfig config)
        {
            View = view;
            Config = config;
            EntryPoint.SubscribeToUpdate(OnUpdate);
        }

        protected virtual void OnUpdate() { }
        protected virtual void OnDispose() { }
    }
}