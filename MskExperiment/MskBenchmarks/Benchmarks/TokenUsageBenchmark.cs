using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MskBenchmarks.Mocks;

using MskCore.ApplicationExtensions;
using MskCore.Chat;
using MskCore.IO;

namespace MskBenchmarks.Benchmarks;

public static class TokenUsageBenchmark
{
    public static async Task Run()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<Program>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddMskCore(configuration); // adjust to whatever signature your extension has

        // Registered after AddMskCore so they win over the real implementations
        services.AddSingleton<ICsvReader, MockCsvReader>();
        services.AddSingleton<IPdfGenerator, MockPdfGenerator>();

        await using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });

        await using var scope = provider.CreateAsyncScope();
        var session = scope.ServiceProvider.GetRequiredService<ChatSession>();
        var monitor = scope.ServiceProvider.GetRequiredService<ITokenUsageMonitor>();

        var sw = Stopwatch.StartNew();
        var reply = await session.SendAsync("What is the capital of France?");
        sw.Stop();

        Console.WriteLine("\n----Token benchmark results----");
        Console.WriteLine($"Elapsed:       {sw.Elapsed.TotalSeconds:F2}s");
        Console.WriteLine($"Input tokens:  {monitor.InputTokens}");
        Console.WriteLine($"Output tokens: {monitor.OutputTokens}");
        Console.WriteLine($"Total tokens:  {monitor.TotalTokens}");
        Console.WriteLine($"\nResponse: {reply}");
    }
}