using BudgetAnimancer;
using UnityEngine;
namespace Sample
{
    public class KnightTest : MonoBehaviour
    {
        public BudgetAnimancerComponent component;
        public AnimationClip attack;
        [Range(0, 2)] public float speed;
        public LinearMixerStateData locomotionData;
        public bool atk;
        protected AnimState attackState;
        protected LinearMixerState locomotionState;
        public AvatarMask attackMask;
        private void Start()
        {
            component.EnsureLayer(1, attackMask);
            component.SetLayerWeight(1, 0);
            locomotionState = component.Layers[0].GetOrAddLinearMixer(locomotionData);
            SwitchToDefault();

            attackState = component.Layers[1].CreateOrGetAnimationState(attack);
            attackState.AddEvent(0.33f, () =>
            {
                Debug.Log("Damage");
            });
            attackState.OnEnd += () =>
            {
                Debug.Log("AttackEvent end");
                //SwitchToDefault();
                component.SetLayerWeight(1, 0, 0.5f);
            };
        }
        private void Update()
        {
            if (atk)
            {
                component.SetLayerWeight(1, 1, 0.5f);
                component.Layers[1].Play(attack);
                atk = false;
            }
            //if (Component.Layers[0].CurrentState == attackState)
            //{
            //    Debug.Log(attackState.NormalizedTime);
            //}
            locomotionState.Parameter = speed;
        }
        void SwitchToDefault()
        {
            component.Layers[0].PlayLinearMixer(locomotionData.Key);
        }
    }
}