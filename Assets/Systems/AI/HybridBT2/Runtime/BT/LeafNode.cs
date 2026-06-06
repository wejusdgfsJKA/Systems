using System;

namespace HybridBT2
{

    public class LeafNode : Node
    {
        protected Func<LeafNode, Blackboard, NodeState> onEvaluate;
        public LeafNode(string name, Func<LeafNode, Blackboard, NodeState> onEvaluate, Action<Node, Blackboard> onEnter = null, Action<Node, Blackboard> onExit = null) : base(name, onEnter, onExit)
        {
            this.onEvaluate = onEvaluate;
        }
        protected override void ExecuteUnderlyingBehaviour(Blackboard context)
        {
            var newState = onEvaluate(this, context);
            SetState(newState, context);
        }
    }
    public abstract class LeafNodeData : NodeData
    {
        protected abstract Func<LeafNode, Blackboard, NodeState> onEvaluate { get; }
        protected override Node GetNodeInternal()
        {
            return new LeafNode(Name, onEvaluate, onEnter, onExit);
        }
    }
}