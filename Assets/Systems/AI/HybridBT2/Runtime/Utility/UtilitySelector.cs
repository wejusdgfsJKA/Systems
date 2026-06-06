using System;
using System.Collections.Generic;
using System.Linq;

namespace HybridBT2
{
    public class UtilitySelector : Composite
    {
        /// <summary>
        /// The child that was running in the previous tick. If the new best child is different, 
        /// the old one will be aborted, and the new one will run. If the new best child is 
        /// the same as the previous one, it will just keep running. If there is no previous 
        /// child, it will just run the new best child.
        /// </summary>
        protected int prevChild = -1;
        protected List<UtilityWrapper> children = new();
        /// <summary>
        /// If we have a previous child, a new child must be better by at least this ammount to take over.
        /// </summary>
        protected float utilityDelta = 0;
        public UtilitySelector(string name, Action<Node, Blackboard> onEnter = null, Action<Node, Blackboard> onExit = null, float utilityDelta = 0) : base(name, onEnter, onExit)
        {
            this.utilityDelta = utilityDelta;
            this.onEnter = onEnter;
            this.onEnter ??= delegate { };
            this.onEnter += (_, _) => prevChild = -1;
        }
        /// <summary>
        /// Only accepts UtilityWrapper.
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="ArgumentNullException">Thrown if the method was passed a null Child.</exception>
        /// <exception cref="ArgumentException">Thrown if the Child could not be cast to a UtilityWrapper.</exception>
        public override void AddChild(Node child)
        {
            if (child == null) throw new ArgumentNullException($"{this} was passed a null Child!");
            if (child is not UtilityWrapper utilityWrapper) throw new ArgumentException($"{this} received non-UtilityWrapper Child {child}.");
            utilityWrapper.Index = children.Count;
            utilityWrapper.Parent = this;
            children.Add(utilityWrapper);
        }
        /// <summary>
        /// Orders children by their utility value, and then executes in order until it 
        /// finds a Child which does not fail. Will abort the previous running action if 
        /// a different one is chosen.
        /// </summary>
        /// <param name="blackboard"></param>
        protected override void ExecuteUnderlyingBehaviour(Blackboard blackboard)
        {
            var sortedChildren = children.OrderBy(x => -x.GetUtility(blackboard));
            int count = -1;
            foreach (var child in sortedChildren)
            {
                count++;

                if (prevChild != -1 && child.Index != prevChild && !child.AlwaysCheck)
                {
                    var currentBest = children[prevChild].GetUtility(blackboard);
                    var newBest = child.GetUtility(blackboard);
                    if (newBest < currentBest + utilityDelta) continue;
                }


                child.Evaluate(blackboard);

                if (child.State == NodeState.FAILURE)
                {
                    if (count == children.Count - 1)
                    {
                        SetState(NodeState.FAILURE, blackboard);
                        return;
                    }
                    SetState(NodeState.RUNNING, blackboard);
                    continue;
                }

                SetState(child.State, blackboard);
                if (prevChild != -1 && prevChild != child.Index) children[prevChild].Abort(blackboard);
                prevChild = child.Index;
                return;
            }
            throw new Exception($"{this} did not finish executing main loop!");
        }
        /// <summary>
        /// Same as the one for regular composites.
        /// </summary>
        /// <param name="indentation"></param>
        /// <returns></returns>
        public override string GetInfo(int indentation)
        {
            var s = base.GetInfo(indentation);
            for (int i = 0; i < children.Count; i++)
            {
                s += "\n" + children[i].GetInfo(indentation + 1);
            }
            return s;
        }
    }
}
