using BenchmarkDotNet.Configs;

using MskBenchmarks.Columns;

namespace MskBenchmarks.Benchmarks;

public class ConcurrencyConfig : ManualConfig
{
    public ConcurrencyConfig()
    {
        AddColumn(new ThroughputColumn());
    }
}
