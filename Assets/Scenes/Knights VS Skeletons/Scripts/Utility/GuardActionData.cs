using UnityEngine;
using UtilityAI;

[CreateAssetMenu(menuName = "UtilityAI/Actions/Guard")]
public class GuardActionData : AIActionData
{
    public override AIAction GetAction(Context context)
    {
        return new SimpleAIAction(Consideration, (_context, _deltaTime) =>
        {
            var guardArea = _context.GetData<Transform[]>(ContextDataKeys.GuardArea);
            if (guardArea == null || guardArea.Length == 0) return;
            if (guardArea.Length == 1)
            {
                var tr = _context.GetData<Transform>(ContextDataKeys.Target);
                if (tr == null) return;
                if (_context.Agent.velocity.magnitude == 0)
                {
                    _context.Transform.LookAt(new Vector3(tr.position.x, _context.Transform.position.y, tr.position.z));
                }
                return;
            }
            int currentPatrolPoint = _context.GetData<int>(ContextDataKeys.CurrentPatrolPoint);
            if (_context.Agent.remainingDistance <= _context.Agent.stoppingDistance)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % guardArea.Length;
                _context.Agent.SetDestination(guardArea[currentPatrolPoint].position);
                _context.SetData(ContextDataKeys.CurrentPatrolPoint, currentPatrolPoint);
            }
        }, (_context) =>
        {
            _context.Agent.isStopped = false;
            _context.Agent.speed = _context.RegularSpeed;
            var guardArea = _context.GetData<Transform[]>(ContextDataKeys.GuardArea);
            if (guardArea == null) return;
            int currentPatrolPoint = 0;
            if (guardArea.Length > 1)
            {
                currentPatrolPoint = _context.GetData<int>(ContextDataKeys.CurrentPatrolPoint);
            }
            _context.Agent.SetDestination(guardArea[currentPatrolPoint].position);
        });
    }
}
