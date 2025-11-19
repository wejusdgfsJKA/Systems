using UnityEngine;
namespace HybridBT.Template
{
    [CreateAssetMenu(menuName = "HybridBT/Template/CurveConsideration")]
    public class CurveConsideration : CurveConsideration<SomeEnum>
    {
        [SerializeField] protected SomeEnum valueKey;
        protected override float GetValueForCurve(Context<SomeEnum> context)
        {
            return context.GetData<float>(valueKey);
        }
    }
}