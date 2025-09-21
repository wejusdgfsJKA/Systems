using EventBus;
using UnityEngine;

public class OnDeathNotifier : MonoBehaviour
{
    protected EntityDied @event;
    protected void Awake()
    {
        @event = new(transform);
    }
    protected void OnDisable()
    {
        EventBus<EntityDied>.Raise(transform.GetInstanceID(), @event);
        EventBus<EntityDied>.ClearBinding(transform.GetInstanceID());
    }
    protected void OnDestroy()
    {
        EventBus<EntityDied>.RemoveBinding(transform.GetInstanceID());
    }
}
