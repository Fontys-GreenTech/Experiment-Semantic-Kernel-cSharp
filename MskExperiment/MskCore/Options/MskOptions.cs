namespace MskCore.Options;

public sealed class MskOptions
{
    public const string Section = "Msk";

    public required string ModelId { get; init; }
    public required string Endpoint { get; init; }
    public required string ApiKey { get; init; }
}