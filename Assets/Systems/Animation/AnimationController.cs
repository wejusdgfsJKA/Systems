using UnityEngine;

namespace Animation
{
    public class AnimationController : AnimationEventReceiver
    {
        protected Animator animator;
        protected override void Awake()
        {
            base.Awake();
            animator = transform.root.GetComponentInChildren<Animator>();
        }
        /// <summary>
        /// Forcibly change to a new animator state, irregardless of transitions.
        /// </summary>
        /// <param name="stateHash">The hash of the state. Use Tools->Generate hashes to get a struct containing all the hashes of an animator controller.</param>
        /// <param name="transitionDuration">How long the transition should take.</param>
        public void ChangeState(int stateHash, float transitionDuration = 0.1f)
        {
            animator.CrossFade(stateHash, transitionDuration);
        }
    }
}