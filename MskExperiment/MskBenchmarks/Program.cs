using BenchmarkDotNet.Running;

using MskBenchmarks.Benchmarks;

namespace MskBenchmarks;

public class Program
{
    public static async Task Main(string[] args)
    {
        // If BenchmarkDotNet args are passed (e.g. --filter *), skip the menu
        if (args.Length > 0)
        {
            await BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAsync(args);
            return;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("What do you want to run?");
            Console.WriteLine("  1) Token usage run");
            Console.WriteLine("  2) BenchmarkDotNet benchmarks");
            Console.WriteLine("  q) Quit");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (choice)
            {
                case "1":
                    await TokenUsageBenchmark.Run();
                    break;

                case "2":
                    await BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAsync(args);
                    break;

                case "q":
                    return;

                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
}

