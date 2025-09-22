using System.Diagnostics; // For Stopwatch
using UnityEngine;

public class CachedTransformCheck : MonoBehaviour
{
    public int nrOfCalls = 100000;
    public int iterations = 5;
    Transform tr;

    private void Awake()
    {
        tr = transform;
    }

    private void Start()
    {
        // Warm-up to avoid JIT compilation overhead
        for (int i = 0; i < 1000; i++)
        {
            var _ = transform.position;
            var __ = tr.position;
        }

        // Benchmark regular transform access
        long totalRegular = 0;
        for (int iter = 0; iter < iterations; iter++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < nrOfCalls; i++)
            {
                var a = transform.position;
            }
            sw.Stop();
            totalRegular += sw.ElapsedTicks;
        }
        UnityEngine.Debug.Log($"Regular transform average time: {(totalRegular / (float)iterations) / Stopwatch.Frequency * 1000f} ms");

        // Benchmark cached transform access
        long totalCached = 0;
        for (int iter = 0; iter < iterations; iter++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < nrOfCalls; i++)
            {
                var a = tr.position;
            }
            sw.Stop();
            totalCached += sw.ElapsedTicks;
        }
        UnityEngine.Debug.Log($"Cached transform average time: {(totalCached / (float)iterations) / Stopwatch.Frequency * 1000f} ms");
    }
}