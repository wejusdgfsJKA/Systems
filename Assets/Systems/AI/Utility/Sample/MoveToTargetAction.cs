using UnityEngine;
using UtilityAI;
[CreateAssetMenu(menuName = "UtilityAI/Actions/MoveToTarget")]
public class MoveToTargetAction : AIAction
{
    public override void Enter(Context context)
    {
        Debug.Log("Entering move");
    }

    public override void Execute(Context context, float deltaTime)
    {
        Debug.Log("Moving");
        var tr = context.GetData<Transform>(ContextKeys.Target);
        context.transform.Translate((tr.position - context.transform.position).normalized * deltaTime);
    }

    public override void Exit(Context context)
    {
        Debug.Log("Exiting move");
    }
}
