using UnityEngine;
using UtilityAI;
[CreateAssetMenu(menuName = "UtilityAI/Actions/Idle")]
public class Idle : AIAction
{
    public override void Enter(Context context)
    {
        Debug.Log("Entering idle");
    }

    public override void Execute(Context context, float deltaTime)
    {
        Debug.Log("Idling");
    }

    public override void Exit(Context context)
    {
        Debug.Log("Exiting idle");
    }
}
