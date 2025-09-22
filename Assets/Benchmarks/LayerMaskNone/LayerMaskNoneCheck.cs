using System.Diagnostics;
using UnityEngine;

public class LayerMaskNoneCheck : MonoBehaviour
{
    public int nrOfCalls = 100000;
    public int iterations = 5;
    Transform tr;
    public LayerMask mask;
    private void Awake()
    {
        tr = transform;
    }
    void Start()
    {
        tr = transform;
        // Warm-up to avoid JIT compilation overhead
        for (int i = 0; i < 1000; i++)
        {
            CheckStopIfLayerMaskNone();
            Check();
        }

        // Benchmark regular transform access
        long totalRegular = 0;
        for (int iter = 0; iter < iterations; iter++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < nrOfCalls; i++)
            {
                Check();
            }
            sw.Stop();
            totalRegular += sw.ElapsedTicks;
        }
        UnityEngine.Debug.Log($"Regular check average time: {(totalRegular / (float)iterations) / Stopwatch.Frequency * 1000f} ms");

        // Benchmark cached transform access
        long totalCached = 0;
        for (int iter = 0; iter < iterations; iter++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < nrOfCalls; i++)
            {
                CheckStopIfLayerMaskNone();
            }
            sw.Stop();
            totalCached += sw.ElapsedTicks;
        }
        UnityEngine.Debug.Log($"Special check average time: {(totalCached / (float)iterations) / Stopwatch.Frequency * 1000f} ms");
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
