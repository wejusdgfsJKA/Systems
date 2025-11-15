using System;
using System.Collections.Generic;
using System.Linq;

namespace HybridBT
{
    public class UtilitySelector<T> : Composite<T>
    {
        protected int prevChild = -1;
        protected List<UtilityWrapper<T>> children = new();
        public UtilitySelector(string name, Action onEnter = null, Action onExit = null) : base(name, onEnter, onExit)
        {
            onEnter += () => prevChild = -1;
        }
        public override void AddChild(Node<T> child)
        {
            if (child is not UtilityWrapper<T> utilityWrapper) throw new ArgumentException($"{this} received non-UtilityWrapper child {child}.");
            utilityWrapper.Index = children.Count;
            utilityWrapper.Parent = this;
            children.Add(utilityWrapper);
        }
        protected override void Execute(Context<T> context)
        {
            var sortedChildren = children.OrderBy(x => -x.GetScore(context));
            int count = -1;
            foreach (var child in sortedChildren)
            {
                count++;
                child.Evaluate(context);
                if (child.State == NodeState.FAILURE)
                {
                    if (count == children.Count - 1)
                    {
                        State = NodeState.FAILURE;
                        return;
                    }
                    State = NodeState.RUNNING;
                    continue;
                }
                State = child.State;
                if (prevChild != -1) children[prevChild].Abort();
                prevChild = child.Index;
                return;
            }
        }
    }
    public class UtilitySelectorData<T> : NodeData<T>
    {
        public List<UtilityWrapperData<T>> Children = new();
        protected override Node<T> GetNode(Context<T> context)
        {
            return new UtilitySelector<T>(Name, onEnter, onExit);
        }
    }
}