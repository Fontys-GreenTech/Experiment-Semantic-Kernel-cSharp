using BenchmarkDotNet.Configs;

namespace MskBenchmarks;

public class ConcurrencyConfig : ManualConfig
{
    public ConcurrencyConfig()
    {
        AddColumn(new ThroughputColumn());
    }
}
