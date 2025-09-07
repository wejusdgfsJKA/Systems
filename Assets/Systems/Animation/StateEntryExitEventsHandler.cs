using EventBus;
using UnityEngine;
namespace Animation
{
    public class StateEntryExitEventsHandler : StateMachineBehaviour
    {
        [SerializeField] protected MyAnimationEvent onEnter, onExit;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            EventBus<MyAnimationEvent>.Raise(animator.transform.root.
                GetInstanceID(), onEnter);
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            EventBus<MyAnimationEvent>.Raise(animator.transform.root.
                GetInstanceID(), onExit);
        }
    }
}