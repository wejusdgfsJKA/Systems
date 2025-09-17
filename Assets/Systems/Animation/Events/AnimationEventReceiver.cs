using EventBus;
using UnityEngine;
namespace Animation
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [Tooltip("Don't put two events with the same ID! Only the first " +
            "will be executed!")]
        [SerializeField] protected AnimEvent[] events;
        protected virtual void Awake()
        {
            EventBus<CustomAnimEvent>.AddActions(transform.root.GetInstanceID(), ReceiveEvent);
        }
        /// <summary>
        /// Receive an event, presumably from a StateEntryEventHandler, StateEventHandler or StateExitEventHandler.
        /// </summary>
        /// <param name="event"></param>
        protected void ReceiveEvent(CustomAnimEvent @event)
        {
            if (events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    if (events[i].ID == @event.ID)
                    {
                        events[i].Event?.Invoke();
                        break;
                    }
                }
            }
        }
        protected virtual void OnDestroy()
        {
            EventBus<CustomAnimEvent>.RemoveBinding(transform.root.GetInstanceID());
        }
    }
}