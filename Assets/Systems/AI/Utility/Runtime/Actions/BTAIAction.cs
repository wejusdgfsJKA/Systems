using BT;

namespace UtilityAI
{
    public class BTAIAction : AIAction
    {
        protected Composite root;
        public BTAIAction(Consideration consideration, Composite root) : base(consideration)
        {
            this.root = root;
        }
        public override void Execute(Context context, float deltaTime)
        {
            root.Evaluate(deltaTime);
        }
        public override void Exit(Context context)
        {
            root.Abort();
        }
    }
}