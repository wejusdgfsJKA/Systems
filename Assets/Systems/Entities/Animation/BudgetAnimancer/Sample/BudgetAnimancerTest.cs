using BudgetAnimancer;
using UnityEngine;
namespace Sample
{
    public class BudgetAnimancerTest : MonoBehaviour
    {
        public BudgetAnimancerComponent component;
        public AnimationClip clip;
        private void Start()
        {
            Debug.Log(component.Play(clip));
        }
    }
}