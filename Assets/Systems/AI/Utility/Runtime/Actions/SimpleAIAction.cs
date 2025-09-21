namespace UtilityAI
{
    public class SimpleAIAction : AIAction
    {
        protected System.Action<Context, float> evaluation;
        protected System.Action<Context> onEnter, onExit;
        public SimpleAIAction(Consideration consideration,
            System.Action<Context, float> evaluation,
            System.Action<Context> onEnter = null,
            System.Action<Context> onExit = null) : base(consideration)
        {
            this.evaluation = evaluation;
            this.onEnter = onEnter;
            this.onExit = onExit;
        }
        public override void Enter(Context context)
        {
            onEnter?.Invoke(context);
        }
        public override void Exit(Context context)
        {
            onExit?.Invoke(context);
        }
        public override void Execute(Context context, float deltaTime)
        {
            evaluation?.Invoke(context, deltaTime);
        }
    }
}