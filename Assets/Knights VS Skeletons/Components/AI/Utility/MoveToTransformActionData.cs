using UnityEngine;
using UnityEngine.AI;
using UtilityAI;
namespace KvS.Utility
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MoveToTransform")]
    public class MoveToTransformActionData : AIActionData
    {
        public ContextDataKeys ContextKey;
        public override AIAction GetAction(Context context)
        {
            return new SimpleAIAction(Consideration, (_context, _deltaTime) =>
            {
                if (_context.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    _context.Agent.SetDestination(_context.GetData<Transform>(ContextKey).position);
                }
            }, (_context) =>
            {
                _context.Agent.isStopped = false;
                _context.Agent.speed = _context.ChaseSpeed;
                _context.Agent.SetDestination(_context.GetData<Transform>(ContextKey).position);
            });
        }
    }
}