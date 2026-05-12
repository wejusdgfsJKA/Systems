using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Nodes/Selector", fileName = "Selector")]
    public class SelectorData : RegularCompositeData
    {
        protected override Node GetNodeInternal()
        {
            return new Selector(Name, onEnter, onExit);
        }
    }
}