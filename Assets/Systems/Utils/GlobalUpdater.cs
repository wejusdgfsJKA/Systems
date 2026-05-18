using System;
using UnityEngine;
namespace Utilities
{
    public sealed class GlobalUpdater : Singleton<GlobalUpdater>
    {
        Action<float> update = delegate { }, lateUpdate = delegate { }, fixedUpdate = delegate { };
        protected override void ClearSingleton()
        {
            base.ClearSingleton();
            update = null;
            lateUpdate = null;
            fixedUpdate = null;
        }
        public void RegisterUpdate(Action<float> action)
        {
            update += action;
        }
        public void UnregisterUpdate(Action<float> action)
        {
            update -= action;
        }
        public void RegisterLateUpdate(Action<float> action)
        {
            lateUpdate += action;
        }
        public void UnregisterLateUpdate(Action<float> action)
        {
            lateUpdate -= action;
        }
        public void RegisterFixedUpdate(Action<float> action)
        {
            fixedUpdate += action;
        }
        public void UnregisterFixedUpdate(Action<float> action)
        {
            fixedUpdate -= action;
        }
        private void Update()
        {
            update?.Invoke(Time.deltaTime);
        }
        private void LateUpdate()
        {
            lateUpdate?.Invoke(Time.deltaTime);
        }
        private void FixedUpdate()
        {
            fixedUpdate?.Invoke(Time.fixedDeltaTime);
        }
    }
}