using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HealAreaInRangeConsideration")]
    public class HealAreaInRangeConsideration : Consideration
    {
        public float MaxDistance = 10;
        public AnimationCurve Curve;
        public ContextDataKeys ContextKey;
        public bool AllowBeyondMaxDistance;
        public override float Evaluate(Context context)
        {
            var tr = context.GetData<Transform>(ContextKey);
            if (tr == null) return 0;
            float dist = Vector3.Distance(context.Transform.position, tr.position);
            if (dist < tr.localScale.x / 2 + context.Transform.localScale.x / 2) return 0;
            if (AllowBeyondMaxDistance || dist <= MaxDistance)
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