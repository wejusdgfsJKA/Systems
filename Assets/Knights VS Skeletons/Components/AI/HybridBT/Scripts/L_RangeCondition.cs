using HybridBT;
using System;
using UnityEngine;
namespace KvS.Hybrid

{
    [CreateAssetMenu(menuName = "HybridBT/KvS/L_RangeCondition")]
    public class L_RangeCondition : ConditionData<KvS_Keys>
    {
        [SerializeField] protected float maxDistance = 2;
        [SerializeField] protected KvS_Keys key;
        public override Func<Context<KvS_Keys>, bool> Function => (context) =>
        {
            float value = context.GetData<float>(key);
            return value <= maxDistance;
        };
    }
}