using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/TransformInRangeConsideration")]
    public class TransformInRangeConsideration : Consideration
    {
        public float MaxDistance = 10;
        public AnimationCurve Curve;
        public ContextDataKeys ContextKey;
        public bool BeyondMaxDistance;
        public override float Evaluate(Context context)
        {
            var tr = context.GetData<Transform>(ContextKey);
            if (tr == null) return 0;
            float dist = Vector3.Distance(context.Transform.position, tr.position);
            if (BeyondMaxDistance || dist <= MaxDistance)
            {
                float result = Curve.Evaluate(dist / MaxDistance);
                return Mathf.Clamp01(result);
            }
            return 0;
        }
        protected void Reset()
        {
            Curve = new AnimationCurve(
                new Keyframe(0f, 1f), // At normalized distance 0, utility is 1
                new Keyframe(1f, 0f)  // At normalized distance 1, utility is 0
            );
        }
    }
}