using UnityEngine.Events;

namespace UtilityAI
{
    public enum ContextDataKeys
    {
        Target,
        DistToTarget,
        MaxDistance,
        PrevTargetPos,
        GuardArea,
        DistToGuardArea,
        CurrentPatrolPoint,
        ClosestHealArea,
        HealAreaDist,
        CurrentHP
    }
    public enum ContextEventKeys
    {
        Attack
    }
    [System.Serializable]
    public struct ContextEvent
    {
        public ContextEventKeys Key;
        public UnityEvent Event;
    }
}