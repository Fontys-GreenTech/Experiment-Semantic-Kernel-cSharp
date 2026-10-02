using BenchmarkDotNet.Running;

namespace MskBenchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<MskFlowBenchmark>();
    }
}

