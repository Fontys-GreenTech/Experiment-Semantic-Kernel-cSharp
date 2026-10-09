using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

using MskBenchmarks.Mocks;

using MskCore.IO;
using MskCore.Plugins;

namespace MskBenchmarks.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(iterationCount: 3, warmupCount: 0)]
[Config(typeof(ConcurrencyConfig))]
public class ConcurrencyBenchmark
{
    private Kernel _kernel = null!;
    private IChatCompletionService _chat = null!;
    private PromptExecutionSettings _settings = null!;

    [Params(1, 3, 5)]
    public int Concurrency { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var mock = new ScriptedChatClient(new()
        {
            ["get_csv_rows"]  = new(),
            ["math_add"]      = new() { ["a"] = 1, ["b"] = 2 },
            ["generate_fiscal_report_pdf"] = new() { ["reportTitle"] = "Fiscal report", ["overviewText"] = "Text", ["adviceText"] = "Text" },
        });

        IChatClient client = new ChatClientBuilder(mock)
            .UseKernelFunctionInvocation()
            .Build();
        
        var builder = Kernel.CreateBuilder();
        builder.Services.AddSingleton<ICsvReader, MockCsvReader>();
        builder.Services.AddSingleton<IPdfGenerator, MockPdfGenerator>();
        builder.Plugins.AddFromType<CsvPlugin>("Csv");
        builder.Plugins.AddFromType<MathPlugin>("Math");
        builder.Plugins.AddFromType<PdfPlugin>("Pdf");
        _kernel = builder.Build();

        _chat = client.AsChatCompletionService();
        _settings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
    }

    [Benchmark(Description = "Concurrent 15 Requests")]
    public async Task RunConcurrentRequestsAsync()
    {
        int totalRequests = 15;
        using var semaphore = new SemaphoreSlim(Concurrency);
        var tasks = new Task[totalRequests];

        for (int i = 0; i < totalRequests; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var history = new ChatHistory("Analyze data.csv, review financials, and generate fiscal report PDF.");
                    await _chat.GetChatMessageContentAsync(history, _settings, _kernel);
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }

        await Task.WhenAll(tasks);
    }
}
