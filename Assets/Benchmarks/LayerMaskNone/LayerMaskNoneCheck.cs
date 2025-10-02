using Benchmarks;
using UnityEngine;
namespace Sample
{
    public class LayerMaskNoneCheck : BenchmarkBase
    {
        Transform tr;
        public LayerMask mask;
        private void Awake()
        {
            tr = transform;
            functions.Add(new("Regular check", Check));
            functions.Add(new("Special check", CheckStopIfLayerMaskNone));
        }
        public void CheckStopIfLayerMaskNone()
        {
            if (mask == 0)
                return;
            Physics.Raycast(tr.position, tr.forward, 100, mask);
        }
        public void Check()
        {
            Physics.Raycast(tr.position, tr.forward, 100, mask);
        }
    }
}