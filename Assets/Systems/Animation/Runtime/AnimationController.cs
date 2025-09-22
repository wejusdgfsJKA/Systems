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
        /// Change to a new animator state using CrossFade.
        /// </summary>
        /// <param name="stateHash">Full hash of the state. Use Tools->Generate hashes to get a struct 
        /// containing all hashes of an animator.</param>
        /// <param name="transitionDuration">How long should the transition take.</param>
        /// <param name="layer">On what layer are we trying to change states.</param>
        /// <param name="canInterruptSelf">If true, will change to the new state even if it is equal to the current one.</param>
        public bool ChangeState(int stateHash, float transitionDuration = 0.25f, int layer = 0, bool canInterruptSelf = false)
        {
            if (!canInterruptSelf)
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
                if (stateInfo.fullPathHash == stateHash) return false;
                if (animator.IsInTransition(layer))
                {
                    stateInfo = animator.GetNextAnimatorStateInfo(layer);
                    if (stateInfo.fullPathHash == stateHash) return false;
                }
            }
            animator.CrossFade(stateHash, transitionDuration);
            return true;
        }
    }
}