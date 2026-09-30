using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MskConsole.ApplicationExtensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddMskConsole();

using var host = builder.Build();
await host.RunAsync();