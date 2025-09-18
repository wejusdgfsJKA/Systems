using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/CurveConsideration")]
    public class CurveConsideration : Consideration
    {
        public AnimationCurve Curve;
        public ContextKeys ContextKey;
        public override float Evaluate(Context context)
        {
            var value = context.GetData<float>(ContextKey);
            float result = Curve.Evaluate(value);
            return Mathf.Clamp01(result);
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