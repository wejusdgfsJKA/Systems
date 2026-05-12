using System;

namespace HybridBT2
{
    /// <summary>
    /// Contains one LeafNode and a consideration.
    /// </summary>
    public class UtilityWrapper : Decorator
    {
        public int Index { get; set; }
        protected readonly Consideration consideration;
        public float Score { get; protected set; }
        public UtilityWrapper(string name, Consideration consideration,
            Action<Blackboard> onEnter, Action<Blackboard> onExit) : base(name, onEnter, onExit)
        {
            this.consideration = consideration;
        }
        public float GetUtility(Blackboard context)
        {
            //score exists only for debugging
            Score = consideration.Evaluate(context);
            return Score;
        }
        protected override void ExecuteUnderlyingBehaviour(Blackboard context)
        {
            Child.Evaluate(context);
            SetState(Child.State, context);
        }
        protected override string DecoratorInfo(int indentation)
        {
            return $"[Utility: {Score}]";
        }
    }
}