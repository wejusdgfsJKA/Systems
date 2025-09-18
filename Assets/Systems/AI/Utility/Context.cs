using BT;
using UnityEngine;
namespace UtilityAI
{
    public class Context : BlackBoard<ContextKeys>
    {
        public readonly Transform transform;
        public Context(Transform transform)
        {
            this.transform = transform;
        }
    }
}