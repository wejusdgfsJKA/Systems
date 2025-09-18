using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/Constant")]
    public class ConstantConsideration : Consideration
    {
        public float Value;
        public override float Evaluate(Context context) => Value;
    }
}