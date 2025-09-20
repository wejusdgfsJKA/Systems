using UnityEngine;
using UnityEngine.Events;
namespace Spawning
{
    public class Spawnable : MonoBehaviour
    {
        [SerializeField] protected UnityEvent init;
        public void Initialize()
        {
            init?.Invoke();
        }
    }
}