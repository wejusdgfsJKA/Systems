using Animation;
using EventBus;
using UnityEngine;
public class AnimationEventsReceiver : MonoBehaviour
{
    private void Awake()
    {
        EventBus<MyAnimationEvent>.AddActions(transform.GetInstanceID(), bla);
    }
    void bla(MyAnimationEvent @event)
    {
        Debug.Log($"{transform} received event with name {@event.Name}");
    }
}
