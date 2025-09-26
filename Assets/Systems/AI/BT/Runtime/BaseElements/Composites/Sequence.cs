namespace BT
{
    /// <summary>
    /// Evaluate all children in order, from left to right.
    /// If found successfull child node, go to next child. BudgetAnimancerState is RUNNING. <br />
    /// If found running child node, state is RUNNING. <br />
    /// If childd node has failed, state is FAILURE. <br />
    /// BudgetAnimancerState is SUCCESS if all child nodes return SUCCESS.
    /// </summary>
    public class Sequence : Composite
    {
        public Sequence(string name = "Sequence") : base(name) { }
        public override bool Evaluate(float deltaTime)
        {
            if (base.Evaluate(deltaTime))
            {
                for (int i = leftmost; i < children.Count; i++)
                {
                    children[i].Evaluate(deltaTime);
                    switch (children[i].State)
                    {
                        case NodeState.RUNNING:
                            state = NodeState.RUNNING;
                            leftmost = i;
                            return false;
                        case NodeState.SUCCESS:
                            state = NodeState.RUNNING;
                            continue;
                        case NodeState.FAILURE:
                            state = NodeState.FAILURE;
                            return false;
                    }
                }
                state = NodeState.SUCCESS;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Doesn't do anything.
        /// </summary>
        /// <param name="child"></param>
        public override void NewLeftmost(Node child) { }
        /// <summary>
        /// If our current running node is no longer valid, we need to abort.
        /// </summary>
        public override void UpdateLeftmost()
        {
            if (state == NodeState.RUNNING)
            {
                Abort();
            }
        }
    }
}