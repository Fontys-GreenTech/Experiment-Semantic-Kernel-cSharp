using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

using MskBenchmarks.Mocks;

using MskCore.Chat;
using MskCore.IO;
using MskCore.Options;
using MskCore.Plugins;

namespace MskBenchmarks;

[MemoryDiagnoser]
[SimpleJob(iterationCount: 3, warmupCount: 0)]
public class RealMskFlowBenchmark
{
    private Kernel _kernel = null!;
    private ChatSession _session = null!;

    [GlobalSetup]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<RealMskFlowBenchmark>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var options = configuration.GetSection(MskOptions.Section).Get<MskOptions>();
        if (options == null || string.IsNullOrWhiteSpace(options.ApiKey))
        {
            throw new InvalidOperationException("Msk:ApiKey is not configured in user secrets or configuration.");
        }

        var builder = Kernel.CreateBuilder();
        builder.Services.AddSingleton<ICsvReader, MockCsvReader>();
        builder.Services.AddSingleton<IPdfGenerator, MockPdfGenerator>();
        builder.Plugins.AddFromType<CsvPlugin>("Csv");
        builder.Plugins.AddFromType<MathPlugin>("Math");
        builder.Plugins.AddFromType<PdfPlugin>("Pdf");

        builder.AddOpenAIChatCompletion(
            modelId: options.ModelId,
            endpoint: new Uri(options.Endpoint),
            apiKey: options.ApiKey);

        _kernel = builder.Build();
        var chat = _kernel.GetRequiredService<IChatCompletionService>();
        _session = new ChatSession(_kernel, chat);
    }

    [Benchmark]
    public async Task RunMskFlowRealAsync()
    {
        await _session.SendAsync("Analyze data.csv, review financials, and generate fiscal report PDF.");
        await Task.Delay(3000);
    }
}
