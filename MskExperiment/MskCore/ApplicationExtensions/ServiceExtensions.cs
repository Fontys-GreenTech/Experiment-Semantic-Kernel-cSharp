using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

using MskCore.Chat;
using MskCore.Options;
using MskCore.Plugins;

namespace MskCore.ApplicationExtensions;

public static class ServiceExtensions
{
    public static void AddMskCore(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(MskOptions.Section).Get<MskOptions>();
        if (string.IsNullOrWhiteSpace(options!.ApiKey))
            throw new InvalidOperationException("Msk:ApiKey is not configured.");

        IKernelBuilder kernelBuilder = services.AddKernel();
        kernelBuilder.AddOpenAIChatCompletion(
            modelId: options.ModelId,
            endpoint: new Uri(options.Endpoint),
            apiKey: options.ApiKey);

        kernelBuilder.Plugins.AddFromType<LightsPlugin>("Lights");

        services.AddScoped<ChatSession>();
    }
}