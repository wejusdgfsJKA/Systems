using System;
using UnityEngine;
namespace Utilities
{
    public interface IUpdatable
    {
        void PerformUpdate();
    }
    public sealed class Updater : MonoBehaviour
    {
        Action update = delegate { };
        public void Register(IUpdatable updatable) => update += updatable.PerformUpdate;
        public void Unregister(IUpdatable updatable) => update -= updatable.PerformUpdate;
        private void Update()
        {
            update();
        }
    }
}