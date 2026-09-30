using Microsoft.Extensions.Logging;

namespace MskConsole;

public static partial class AppRunnerLoggerMethods
{
    [LoggerMessage(LogLevel.Error, "Wrote '{Message}'")]
    public static partial void WroteHello(this ILogger<AppRunner> logger, string message);
}