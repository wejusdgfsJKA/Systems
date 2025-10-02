using UnityEngine;
namespace Benchmarks
{
    public class CachedTransformCheck : BenchmarkBase
    {
        Transform tr;

        private void Awake()
        {
            tr = transform;
            functions.Add(new("Regular transform", () =>
            {
                var _ = transform.position;
            }));
            functions.Add(new("Cached transform", () =>
            {
                var _ = tr.position;
            }));
        }
    }
}