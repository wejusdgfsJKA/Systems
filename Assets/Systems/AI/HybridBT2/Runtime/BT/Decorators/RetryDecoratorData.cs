using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Decorators/Retry", fileName = "Retry")]
    public class RetryDecoratorData : DecoratorData
    {
        [Tooltip("Set to negative for infinity, otherwise the repeater will return success after repeating MaxNumber times.")]
        public int MaxNumber = -1;
        protected override Node GetNodeInternal()
        {
            return new Retry(Name, MaxNumber, onEnter, onExit);
        }
    }
}