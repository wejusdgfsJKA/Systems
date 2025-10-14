using UnityEngine;
namespace Benchmarks
{
    public class DistanceVsSqDistance : BenchmarkBase
    {
        [SerializeField] Transform other;
        Transform tr;
        private void Awake()
        {
            tr = transform;
            functions.Add(new("Distance", () =>
            {
                var _ = Vector3.Distance(other.position, tr.position);
            }));
            functions.Add(new("Magnitude", () =>
            {
                var _ = Vector3.Magnitude(other.position - tr.position);
            }));
            functions.Add(new("Squared magnitude", () =>
            {
                var _ = Vector3.SqrMagnitude(other.position - tr.position);
            }));
        }
    }
}