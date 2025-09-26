using BudgetAnimancer;
using UnityEngine;
namespace Sample
{
    public class BudgetAnimancerTest : MonoBehaviour
    {
        public BudgetAnimancerComponent component;
        public AnimationClip idle, walk, attack;
        public float speed;
        public bool atk;
        public BudgetAnimancer.AnimationState state;
        private void Start()
        {
            component.Play(idle);
            state = component.CreateOrGetState(attack);
            state.AddEvent(0.33f, () =>
            {
                Debug.Log("Damage");
            });
            state.OnEnd += () =>
            {
                Debug.Log("Attack end");
                component.Play(idle);
            };
        }
        private void Update()
        {
            if (atk)
            {
                component.Play(attack);
                atk = false;
            }
            if (component.Layers[0].CurrentState == state)
            {
                Debug.Log(state.NormalizedTime);
            }
        }
    }
}