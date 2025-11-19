using HybridBT;
using System;
using UnityEngine;
namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/UpdateBlackboardService")]
    public class UpdateBlackboardService : ServiceData<KvS_Keys>
    {
        public override Action<Context<KvS_Keys>> Action => (context) =>
        {
            var myPos = context.Transform.position;
            var guardArea = context.GetData<Transform[]>(KvS_Keys.GuardArea);
            if (guardArea != null && guardArea.Length > 0)
            {
                int currentPoint = 0;
                if (guardArea.Length > 1)
                {
                    currentPoint = context.GetData<int>(KvS_Keys.CurrentPatrolPoint);
                }
                context.SetData(KvS_Keys.DistToGuardArea,
                    Vector3.Distance(myPos, guardArea[currentPoint].position));
            }
            var healArea = context.GetData<Transform>(KvS_Keys.ClosestHealArea);
            if (context.GetData<float>(KvS_Keys.CurrentHPPercentage) < 1)
            {
                context.SetData(KvS_Keys.HealAreaDist, Vector3.Distance(healArea.position, myPos));
            }
        };
    }
}