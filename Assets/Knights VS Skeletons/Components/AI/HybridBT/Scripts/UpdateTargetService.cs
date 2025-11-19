using HybridBT;
using System;
using UnityEngine;
namespace KvS.Hybrid

{
    [CreateAssetMenu(menuName = "HybridBT/KvS/UpdateTargetService")]
    public class UpdateTargetService : ServiceData<KvS_Keys>
    {
        public override Action<Context<KvS_Keys>> Action => (context) =>
        {
            var myPos = context.Transform.position;
            var target = context.GetData<Transform>(KvS_Keys.Target);
            if (target != null)
            {
                context.SetData(KvS_Keys.DistToTarget, Vector3.Distance(myPos, target.position));
            }
        };
    }
}