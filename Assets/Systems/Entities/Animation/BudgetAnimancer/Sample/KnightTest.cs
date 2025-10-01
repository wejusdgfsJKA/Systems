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
        protected int locomotionKey = 0;
        private void Start()
        {
            locomotionState = component.Layers[0].GetOrAddLinearMixer(locomotionKey, locomotionData);
            SwitchToDefault();
            attackState = component.CreateOrGetState(attack);
            attackState.AddEvent(0.33f, () =>
            {
                Debug.Log("Damage");
            });
            attackState.OnEnd += () =>
            {
                Debug.Log("Attack end");
                SwitchToDefault();
            };
        }
        private void Update()
        {
            if (atk)
            {
                component.Play(attack);
                atk = false;
            }
            //if (component.Layers[0].CurrentState == attackState)
            //{
            //    Debug.Log(attackState.NormalizedTime);
            //}
            locomotionState.Parameter = speed;
        }
        void SwitchToDefault()
        {
            component.Layers[0].PlayLinearMixer(locomotionKey);
        }
    }
}