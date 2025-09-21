using UnityEngine;

namespace UtilityAI
{
    public abstract class AIActionData : ScriptableObject
    {
        public Consideration Consideration;
        public abstract AIAction GetAction(Context context);
    }
}