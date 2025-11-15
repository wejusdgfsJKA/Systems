using System;

namespace HybridBT
{

    public class LeafNode<T> : Node<T>
    {
        protected Action<Context<T>> onEvaluate;
        public LeafNode(string name, Action<Context<T>> onEvaluate, Action onEnter = null, Action onExit = null) : base(name, onEnter, onExit)
        {
            this.onEvaluate = onEvaluate;
        }
        protected override void Execute(Context<T> context)
        {
            onEvaluate(context);
        }
    }
    public abstract class LeafNodeData<T> : NodeData<T>
    {
        protected abstract Action<Context<T>> onEvaluate { get; }
        protected override Node<T> GetNode(Context<T> context)
        {
            return new LeafNode<T>(Name, onEvaluate, onEnter, onExit);
        }
    }
}