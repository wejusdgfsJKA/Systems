using EventBus;
using HP;
using Tags;
using UnityEngine;
public class HealArea : Taggable<AreaType>
{
    [SerializeField] int healAmount = 5;
    protected TakeDamage @event;
    private void Awake()
    {
        @event = new(-healAmount, null);
    }
    private void OnTriggerEnter(Collider other)
    {
        EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), @event);
    }
}
