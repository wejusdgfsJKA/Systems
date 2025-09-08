using EventBus;
using UnityEngine;
namespace Animation
{
    public class StateEventHandler : StateMachineBehaviour
    {
        [Range(0, 1)][SerializeField] protected float triggerTime;
        protected bool triggered;
        [SerializeField] protected CustomAnimEvent @event;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            triggered = false;
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float time = stateInfo.normalizedTime % 1;
            if (!triggered && time >= triggerTime)
            {
                //trigger Event
                triggered = true;
                EventBus<CustomAnimEvent>.Raise(animator.transform.root.GetInstanceID(), @event);
            }
        }
    }
}