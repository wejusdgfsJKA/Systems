using System;

namespace HybridBT2
{

    public class LeafNode : Node
    {
        protected Func<Blackboard, NodeState> onEvaluate;
        public LeafNode(string name, Func<Blackboard, NodeState> onEvaluate, Action<Blackboard> onEnter = null, Action<Blackboard> onExit = null) : base(name, onEnter, onExit)
        {
            this.onEvaluate = onEvaluate;
        }
        protected override void ExecuteUnderlyingBehaviour(Blackboard context)
        {
            var newState = onEvaluate(context);
            SetState(newState, context);
        }
    }
    public abstract class LeafNodeData : NodeData
    {
        protected abstract Func<Blackboard, NodeState> onEvaluate { get; }
        protected override Node GetNodeInternal()
        {
            return new LeafNode(Name, onEvaluate, onEnter, onExit);
        }
    }
}