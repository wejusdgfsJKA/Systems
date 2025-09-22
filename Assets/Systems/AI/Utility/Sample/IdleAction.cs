using UnityEngine;
using UtilityAI;
namespace Sample
{
    [CreateAssetMenu(menuName = "Sample/UtilityAI/Actions/Idle")]
    public class Idle : AIActionData
    {
        public override AIAction GetAction(Context context)
        {
            return new SimpleAIAction(Consideration, (_context, _deltaTime) =>
            {
                Debug.Log("Idling");
            }, (_context) =>
            {
                Debug.Log("Entering idle");
            }, (_context) =>
            {
                Debug.Log("Exiting idle");
            });
        }
    }
}