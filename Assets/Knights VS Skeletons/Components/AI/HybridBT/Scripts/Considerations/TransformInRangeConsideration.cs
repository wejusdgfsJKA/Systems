using HybridBT;
using UnityEngine;

namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/TransformInRangeConsideration")]
    public class TransformInRangeConsideration : CurveConsideration<KvS_Keys>
    {
        [SerializeField] protected KvS_Keys transformKey;
        public float MaxDistance = 10;
        public bool AllowBeyondMaxDistance;
        protected override float GetValueForCurve(Context<KvS_Keys> context)
        {
            var tr = context.GetData<Transform>(transformKey);
            if (tr == null) return 0;
            float dist = Vector3.Distance(context.Transform.position, tr.position);
            if (AllowBeyondMaxDistance || dist <= MaxDistance)
            {
                float result = Curve.Evaluate(dist / MaxDistance);
                return Mathf.Clamp01(result);
            }
            return 0;
        }
    }
}