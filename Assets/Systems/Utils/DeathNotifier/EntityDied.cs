using EventBus;
using UnityEngine;

public struct EntityDied : IEvent
{
    public Transform Entity;
    public EntityDied(Transform tr)
    {
        Entity = tr;
    }
}