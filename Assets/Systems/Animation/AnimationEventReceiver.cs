using EventBus;
using UnityEngine;
namespace Animation
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [Tooltip("Don't put two events with the same name! Only the first " +
            "will be executed!")]
        [SerializeField] protected AnimEvent[] events;
        protected virtual void Awake()
        {
            EventBus<CustomAnimEvent>.AddActions(transform.root.GetInstanceID(), ReceiveEvent);
        }
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
    }
}