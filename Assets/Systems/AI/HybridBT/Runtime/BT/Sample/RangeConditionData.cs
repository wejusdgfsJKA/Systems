using HybridBT;
using System;
using UnityEngine;
namespace Sample
{
    [CreateAssetMenu(menuName = "Sample/HybridBT/RangeCondition")]
    public class RangeConditionData : ConditionData<TestBTKeys>
    {
        public TestBTKeys RangeKey;
        public override Func<Context<TestBTKeys>, bool> Func => (c) => Vector3.Distance(
            c.GetValue<Transform>(TestBTKeys.Goober).position, c.GetValue<Transform>(TestBTKeys.Self).position)
        <= c.GetValue<float>(RangeKey);
    }
}