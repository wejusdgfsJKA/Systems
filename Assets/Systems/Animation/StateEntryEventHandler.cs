using EventBus;
using UnityEngine;
namespace Animation
{
    public class StateEntryEventHandler : StateMachineBehaviour
    {
        [SerializeField] protected CustomAnimEvent onEnter;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            EventBus<CustomAnimEvent>.Raise(animator.transform.root.
                GetInstanceID(), onEnter);
        }
    }
}