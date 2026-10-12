namespace MskCore.Chat;

public sealed class TokenUsageMonitor : ITokenUsageMonitor
{
    public int InputTokens { get; set; } = 0;
    public int OutputTokens { get; set; } = 0;
    public int TotalTokens { get; set; } = 0;
}