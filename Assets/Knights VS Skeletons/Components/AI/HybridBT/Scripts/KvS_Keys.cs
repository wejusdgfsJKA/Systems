using EventBus;
namespace KvS
{
    /// <summary>
    /// Enum keys for anything related to Knights VS Skeletons.
    /// </summary>
    public enum KvS_Keys
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
        CurrentHPPercentage
    }
    public class AttackEvent : IEvent { }
}