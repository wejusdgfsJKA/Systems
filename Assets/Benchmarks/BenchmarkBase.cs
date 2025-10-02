using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace Benchmarks
{
    public abstract class BenchmarkBase : MonoBehaviour
    {
        public int nrOfCalls = 100000;
        public int iterations = 5;
        public List<Tuple<string, Action>> functions = new();
        protected virtual void Start()
        {
            //warmup
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < functions.Count; j++)
                {
                    functions[j].Item2();
                }
            }

            for (int k = 0; k < functions.Count; k++)
            {
                long totalCached = 0;
                for (int iter = 0; iter < iterations; iter++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    for (int i = 0; i < nrOfCalls; i++)
                    {
                        functions[k].Item2();
                    }
                    sw.Stop();
                    totalCached += sw.ElapsedTicks;
                }
                UnityEngine.Debug.Log($"{functions[k].Item1} average time: {totalCached / (float)iterations / Stopwatch.Frequency * 1000f} ms");
            }
        }
    }
}