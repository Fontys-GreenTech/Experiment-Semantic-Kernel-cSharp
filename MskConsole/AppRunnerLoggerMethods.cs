using Microsoft.Extensions.Logging;

namespace MskConsole;

public static partial class AppRunnerLoggerMethods
{
    [LoggerMessage(LogLevel.Information, "Application started, chat service running")]
    public static partial void Entry(this ILogger<AppRunner> logger);
}