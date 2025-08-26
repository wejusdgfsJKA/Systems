using EventBus;
using UnityEngine;
namespace Checkpoint
{
    public class Checkpoint : MonoBehaviour
    {
        public static Checkpoint ActiveCheckpoint { get; protected set; }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetCheckpoint()
        {
            ActiveCheckpoint = null;
        }
        protected virtual void OnEnable()
        {
            ActiveCheckpoint = this;
            EventBus<CheckpointReached>.Raise(0, new CheckpointReached());
        }
    }
}