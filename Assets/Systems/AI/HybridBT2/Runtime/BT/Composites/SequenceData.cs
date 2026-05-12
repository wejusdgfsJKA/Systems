using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Nodes/Sequence", fileName = "Sequence")]
    public class SequenceData : RegularCompositeData
    {
        protected override Node GetNodeInternal()
        {
            return new Sequence(Name, onEnter, onExit);
        }
    }
}