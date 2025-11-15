using UnityEngine;

namespace HybridBT
{
    public abstract class Consideration<T> : ScriptableObject
    {
        public abstract float Evaluate(Context<T> context);
    }
    public class ConstantConsideration<T> : Consideration<T>
    {
        public float Value;
        public override float Evaluate(Context<T> context) => Value;
    }
    public abstract class CurveConsideration<T> : Consideration<T>
    {
        public AnimationCurve Curve;
        public bool ReturnZeroForInfinity = true;
        protected abstract float GetValueForCurve(Context<T> context);
        public override float Evaluate(Context<T> context)
        {
            var value = GetValueForCurve(context);
            if (ReturnZeroForInfinity && (value == Mathf.Infinity || value == Mathf.NegativeInfinity)) return 0;
            return Mathf.Clamp01(Curve.Evaluate(value));
        }
        protected virtual void Reset()
        {
            Curve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(1f, 0f)
            );
        }
    }
}