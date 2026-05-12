using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Nodes/Parallel", fileName = "Parallel")]
    public class ParallelNodeData : NodeData
    {
        public NodeData LeftChild, RightChild;
        protected override Node GetNodeInternal()
        {
            return new ParallelNode(Name, LeftChild.GetNodeExternal(), RightChild.GetNodeExternal(), onEnter, onExit);
        }
    }
}