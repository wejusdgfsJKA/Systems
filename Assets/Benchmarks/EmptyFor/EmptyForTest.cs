using Benchmarks;
namespace Sample
{
    public class EmptyForTest : BenchmarkBase
    {
        int a = 0;
        private void Awake()
        {
            functions.Add(new("Empty for ", () =>
            {
                for (int i = 0; i < a; i++)
                {
                    var _ = i;
                }
            }));
            functions.Add(new("Empty for with check", () =>
            {
                if (a == 0) return;
                for (int i = 0; i < a; i++)
                {
                    var _ = i;
                }
            }));
        }
    }
}