using UnityEngine;
using UnityEngine.Events;
namespace Spawning.Pooling
{
    public class Poolable : Spawnable
    {
        [SerializeField] protected UnityEvent reset;
        public Manager Manager { get; set; }
        public void ResetObject()
        {
            reset?.Invoke();
        }
        public virtual void OnDisable()
        {
            Manager?.ReturnToPool(this);
        }
    }
}