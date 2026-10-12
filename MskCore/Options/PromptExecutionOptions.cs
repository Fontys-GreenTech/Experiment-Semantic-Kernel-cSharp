namespace MskCore.Options;

public sealed record PromptExecutionOptions
{
    public required float Temperature { get; set; }
}