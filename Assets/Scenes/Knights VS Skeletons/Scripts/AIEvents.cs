using EventBus;
using UnityEngine;

public struct AssignGuardArea : IEvent
{
    public Transform[] GuardArea;
    public AssignGuardArea(Transform[] GuardArea)
    {
        if (GuardArea == null || GuardArea.Length == 0)
        {
            throw new System.ArgumentException("Guard area has no guardArea!");
        }
        this.GuardArea = GuardArea;
    }
}
