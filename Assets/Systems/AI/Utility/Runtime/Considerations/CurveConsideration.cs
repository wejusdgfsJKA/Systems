using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/CurveConsideration")]
    public class CurveConsideration : Consideration
    {
        public AnimationCurve Curve;
        public ContextDataKeys ContextKey;
        public bool returnZeroForInfinity = true;
        public override float Evaluate(Context context)
        {
            var value = context.GetData<float>(ContextKey);
            if (returnZeroForInfinity && (value == Mathf.Infinity || value == Mathf.NegativeInfinity)) return 0;
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