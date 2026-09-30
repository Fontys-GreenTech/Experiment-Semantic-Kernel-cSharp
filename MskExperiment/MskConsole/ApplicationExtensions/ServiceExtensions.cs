using Microsoft.Extensions.DependencyInjection;

using MskCore.ApplicationExtensions;

namespace MskConsole.ApplicationExtensions;

public static class ServiceExtensions
{
    public static void AddMskConsole(this IServiceCollection services)
    {
        services.AddHostedService<AppRunner>();
        
        services.AddDependencies();
    }

    private static void AddDependencies(this IServiceCollection services)
    {
        services.AddMskCore();
    }
}