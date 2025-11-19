using HybridBT;
using UnityEngine;

namespace KvS.Hybrid
{
    [CreateAssetMenu(menuName = "HybridBT/KvS/CurveConsideration")]
    public class CurveConsideration : CurveConsideration<KvS_Keys>
    {
        [SerializeField] protected KvS_Keys valueKey;
        protected override float GetValueForCurve(Context<KvS_Keys> context)
        {
            return context.GetData<float>(valueKey);
        }
    }
}