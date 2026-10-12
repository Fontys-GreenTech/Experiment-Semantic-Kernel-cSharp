namespace MskCore.Chat;

public interface ITokenUsageMonitor
{
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int TotalTokens { get; set; }
}