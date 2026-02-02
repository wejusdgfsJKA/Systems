using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class AnimationController : AnimationEventReceiver
    {
        protected Animator animator;
        protected Dictionary<int, int> statePriorities = new();
        protected override void Awake()
        {
            base.Awake();
            animator = transform.GetComponentInChildren<Animator>();
        }
        #region States
        public int GetStatePriority(int stateHash)
        {
            return statePriorities.TryGetValue(stateHash, out var priority) ? priority : 1;
        }

        /// <summary>
        /// Change to a new Component attackState using CrossFade.
        /// </summary>
        /// <param name="stateHash">The full path hash of the attackState.</param>
        /// <param name="transitionDuration">How long should the transition take.</param>
        /// <param name="layer">The layer where the attackState change should occur.</param>
        /// <param name="ignorePriority">If true, the transition will still occur even if the new 
        /// state has a lower priority than the current state.</param>
        /// <param name="canInterruptSelf">If true, the transition will still occur even when 
        /// transitioning to the same state.</param>
        /// <returns>True if the transition has occured successfully.</returns>
        public bool ChangeAnimatorState(int stateHash, float transitionDuration = 0.25f, int layer = 0, bool ignorePriority = false, bool canInterruptSelf = false)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(layer);
            AnimatorStateInfo? currentFutureState = animator.IsInTransition(layer) ?
                animator.GetNextAnimatorStateInfo(layer) : null;

            if (!ignorePriority)
            {
                var currentStatePriority = GetStatePriority(currentState.fullPathHash);
                var newStatePriority = GetStatePriority(stateHash);
                if (newStatePriority < currentStatePriority) return false;
                if (currentFutureState != null && GetStatePriority(currentFutureState.Value.fullPathHash)
                    > newStatePriority) return false;
            }

            if (!canInterruptSelf)
            {
                if (currentState.fullPathHash == stateHash) return false;
                if (currentFutureState != null && currentState.fullPathHash == stateHash) return false;
            }

            animator.CrossFade(stateHash, transitionDuration);
            return true;
        }
        #endregion

        #region Parameters
        public void SetValue<T>(int paramHash, T newValue)
        {
            switch (newValue)
            {
                case int i:
                    animator.SetInteger(paramHash, i);
                    break;
                case float f:
                    animator.SetFloat(paramHash, f);
                    break;
                case bool b:
                    animator.SetBool(paramHash, b);
                    break;
                default:
                    break;
            }
        }
        #endregion

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