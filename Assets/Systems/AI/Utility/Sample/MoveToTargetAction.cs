using UnityEngine;
using UtilityAI;
[CreateAssetMenu(menuName = "UtilityAI/Actions/MoveToTarget")]
public class MoveToTargetAction : AIActionData
{
    public override AIAction GetAction(Context context)
    {
        return new SimpleAIAction(Consideration, (_context, _deltaTime) =>
        {
            Debug.Log("Moving");
            var tr = _context.GetData<Transform>(ContextDataKeys.Target);
            _context.Transform.Translate((tr.position - _context.Transform.position).normalized * _deltaTime);
        }, (_context) =>
        {
            Debug.Log("Entering move");
        }, (_context) =>
        {
            Debug.Log("Exiting move");
        });
    }
}
