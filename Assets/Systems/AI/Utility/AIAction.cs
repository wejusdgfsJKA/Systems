using UnityEngine;

namespace UtilityAI
{
    public abstract class AIAction : ScriptableObject
    {
        public Consideration Consideration;
        public float Evaluate(Context context)
        {
            return Consideration.Evaluate(context);
        }
        public virtual void Initialize(Context context) { }
        public virtual void Enter(Context context) { }
        public abstract void Execute(Context context, float deltaTime);
        public virtual void Exit(Context context) { }
    }
}