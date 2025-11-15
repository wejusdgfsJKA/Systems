using System;

namespace HybridBT
{
    public class UtilityWrapper<T> : Node<T>
    {
        public int Index { get; set; }
        protected readonly Consideration<T> consideration;
        protected readonly Node<T> child;
        public UtilityWrapper(string name, Node<T> child, Consideration<T> consideration, Action onEnter, Action onExit) : base(name, onEnter, onExit)
        {
            this.child = child;
            child.Parent = this;
            this.consideration = consideration;
        }
        public float GetScore(Context<T> context) => consideration.Evaluate(context);
        protected override void Execute(Context<T> context)
        {
            child.Evaluate(context);
            State = child.State;
        }
    }
    public class UtilityWrapperData<T> : NodeData<T>
    {
        public Consideration<T> Consideration;
        public NodeData<T> Child;
        protected override Node<T> GetNode(Context<T> context)
        {
            return new UtilityWrapper<T>(Name, Child.ObtainNode(context), Consideration, onEnter, onExit);
        }
    }
}