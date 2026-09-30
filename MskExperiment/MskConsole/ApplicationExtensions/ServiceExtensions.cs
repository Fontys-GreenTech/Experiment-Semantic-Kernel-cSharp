using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MskCore.ApplicationExtensions;

namespace MskConsole.ApplicationExtensions;

public static class ServiceExtensions
{
    public static void AddMskConsole(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<AppRunner>();
        
        services.AddDependencies(configuration);
    }

    private static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMskCore(configuration);
    }
}