using Microsoft.Extensions.Hosting;

using MskConsole.ApplicationExtensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMskConsole(builder.Configuration);

using var host = builder.Build();
await host.RunAsync();