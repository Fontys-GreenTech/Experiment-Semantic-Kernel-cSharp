using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace MskBenchmarks.Columns;

public class ThroughputColumn : IColumn
{
    public string Id => nameof(ThroughputColumn);
    public string ColumnName => "Throughput";
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory => 0;
    public bool IsNumeric => true;
    public UnitType UnitType => UnitType.Dimensionless;
    public string Legend => "Requests per second";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var report = summary.Reports.FirstOrDefault(r => r.BenchmarkCase == benchmarkCase);
        var meanNs = report?.ResultStatistics?.Mean ?? 0;
        if (meanNs == 0) return "N/A";
        
        // 1 operation = 15 requests
        double reqsPerSec = (15.0 * 1_000_000_000.0) / meanNs;
        return reqsPerSec.ToString("N0");
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);
    public bool IsAvailable(Summary summary) => true;
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
}
