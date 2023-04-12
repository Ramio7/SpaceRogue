using System;
using UnityEngine;
using Zenject;

namespace Services
{
    public sealed class Updater : ITickable, IFixedTickable, ILateTickable
    {
        private event Action OnUpdate = () => { };
        private event Action<float> OnDeltaTimeUpdate = _ => { };
        private event Action OnFixedUpdate = () => { };
        private event Action<float> OnDeltaTimeFixedUpdate = _ => { };
        private event Action OnLateUpdate = () => { };
    
        public void SubscribeToUpdate(Action callback) => OnUpdate += callback;
        public void UnsubscribeFromUpdate(Action callback) => OnUpdate -= callback;
        public void SubscribeToUpdate(Action<float> callback) => OnDeltaTimeUpdate += callback;
        public void UnsubscribeFromUpdate(Action<float> callback) => OnDeltaTimeUpdate -= callback;
    
        public void SubscribeToFixedUpdate(Action callback) => OnFixedUpdate += callback;
        public void UnsubscribeFromFixedUpdate(Action callback) => OnFixedUpdate -= callback;  
        public void SubscribeToFixedUpdate(Action<float> callback) => OnDeltaTimeFixedUpdate += callback;
        public void UnsubscribeFromFixedUpdate(Action<float> callback) => OnDeltaTimeFixedUpdate -= callback;   
    
        public void SubscribeToLateUpdate(Action callback) => OnLateUpdate += callback;
        public void UnsubscribeFromLateUpdate(Action callback) => OnLateUpdate -= callback;

        public void Tick()
        {
            OnUpdate.Invoke();
            OnDeltaTimeUpdate.Invoke(Time.deltaTime);
        }

        public void FixedTick()
        {
            OnFixedUpdate.Invoke();
            OnDeltaTimeFixedUpdate.Invoke(Time.fixedDeltaTime);
        }

        public void LateTick()
        {
            OnLateUpdate.Invoke();
        }
    }
}