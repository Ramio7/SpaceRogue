namespace Gameplay.Asteroid.Behaviour
{
    public abstract class AsteroidBehaviour
    {
        protected readonly AsteroidView _view;
        protected readonly AsteroidBehaviourConfig _config;

        public void Dispose()
        {
            OnDispose();
            EntryPoint.UnsubscribeFromUpdate(OnUpdate);
        }

        protected AsteroidBehaviour(AsteroidView view, AsteroidBehaviourConfig config)
        {
            _view = view;
            _config = config;
            EntryPoint.SubscribeToUpdate(OnUpdate);
        }

        protected virtual void OnUpdate() { }
        protected virtual void OnDispose() { }
    }
}