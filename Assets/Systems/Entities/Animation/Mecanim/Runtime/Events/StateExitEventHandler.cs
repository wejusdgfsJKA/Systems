using EventBus;
using UnityEngine;
namespace Animation
{
    public class StateExitEventHandler : StateMachineBehaviour
    {
        [SerializeField] protected CustomAnimEvent onExit;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            EventBus<CustomAnimEvent>.Raise(animator.transform.root.
                GetInstanceID(), onExit);
        }
    }
}