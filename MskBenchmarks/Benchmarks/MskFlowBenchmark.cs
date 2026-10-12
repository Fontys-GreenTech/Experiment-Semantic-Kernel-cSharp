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
public class MskFlowBenchmark
{
    private Kernel _kernel = null!;
    private IChatCompletionService _chat = null!;
    private PromptExecutionSettings _settings = null!;

    [GlobalSetup]
    public void Setup()
    {
        var mock = new ScriptedChatClient(new()
        {
            ["get_csv_rows"]  = new(),
            ["math_add"]      = new() { ["a"] = 1, ["b"] = 2 },
            ["generate_fiscal_report_pdf"] = new() { ["reportTitle"] = "Fiscal report", ["overviewText"] = "Text", ["adviceText"] = "Text" },
        });

        // SK's own invoking client wraps the mock, same as a real connector would
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

    [Benchmark]
    public async Task RunMskFlowMockAsync()
    {
        var history = new ChatHistory("Analyze data.csv, review financials, and generate fiscal report PDF.");
        await _chat.GetChatMessageContentAsync(history, _settings, _kernel);
    }
}