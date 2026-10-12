using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MskCore.Chat;

namespace MskConsole;

public sealed class AppRunner(ILogger<AppRunner> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        using var scope = scopeFactory.CreateScope();
        var session = scope.ServiceProvider.GetRequiredService<ChatSession>();
        
        logger.Entry();

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Write("User > ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) break;

            Console.WriteLine("Assistant > " + await session.SendAsync(input, stoppingToken));
        }
    }
}