using UnityEngine;

namespace Animation
{
    public class AnimationController : AnimationEventReceiver
    {
        protected Animator animator;
        protected override void Awake()
        {
            base.Awake();
            animator = transform.GetComponentInChildren<Animator>();
        }
        /// <summary>
        /// Change to a new component attackState using CrossFade.
        /// </summary>
        /// <param name="stateHash">The full path hash of the attackState.</param>
        /// <param name="transitionDuration">How long should the transition take.</param>
        /// <param name="layer">The layer where the attackState change should occur.</param>
        /// <param name="canInterruptSelf">If true, the transition will still occur</param>
        /// <returns>True if the transition has occured successfully.</returns>
        public bool ChangeAnimatorState(int stateHash, float transitionDuration = 0.25f, int layer = 0, bool canInterruptSelf = false)
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
        protected virtual void OnDrawGizmosSelected()
        {
            var tr = animator ? animator.transform : transform;
            DrawChildren(tr);
        }
        protected virtual void DrawChildren(Transform tr)
        {
            for (int i = 0; i < tr.childCount; i++)
            {
                var child = tr.GetChild(i);
                Gizmos.DrawSphere(child.position, 0.1f);
                Gizmos.DrawLine(tr.position, child.position);
                DrawChildren(child);
            }
        }
    }
}