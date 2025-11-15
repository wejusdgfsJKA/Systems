using HybridBT;
using UnityEngine;
namespace Sample
{
    public class TestBT : BT<TestBTKeys>
    {
        [SerializeField] Transform goober;
        [SerializeField] float meleeRange = 1, hipfireRange = 4;
        protected override void SetupBlackboard()
        {
            SetValue(TestBTKeys.Goober, goober);
            SetValue(TestBTKeys.Self, transform);
            SetValue(TestBTKeys.MeleeRange, meleeRange);
            SetValue(TestBTKeys.HipfireRange, hipfireRange);
        }
    }
}