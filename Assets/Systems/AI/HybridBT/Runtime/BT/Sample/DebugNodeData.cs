using HybridBT;
using System;
using UnityEngine;
namespace Sample
{
    [CreateAssetMenu(menuName = "Sample/HybridBT/DebugNode")]
    public class DebugNodeData : LeafNodeData<TestBTKeys>
    {
        public string Text;
        protected override Action<Context<TestBTKeys>> onEvaluate => (c) => Debug.Log(Text);
    }
}