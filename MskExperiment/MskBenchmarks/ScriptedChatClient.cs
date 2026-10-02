using Microsoft.Extensions.AI;

namespace MskBenchmarks;

public sealed class ScriptedChatClient : IChatClient
{
    private readonly Dictionary<string, Dictionary<string, object?>> _args;

    public ScriptedChatClient(Dictionary<string, Dictionary<string, object?>> args) => _args = args;

    public Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken ct = default)
    {
        if (messages.Last().Role == ChatRole.Tool)
            return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant, "Mock LLM response.")));

        return Task.FromResult(BuildToolCalls(options));
    }

    private ChatResponse BuildToolCalls(ChatOptions? options)
    {
        var tools = options?.Tools ?? throw new InvalidOperationException("No tools offered.");
        var contents = new List<AIContent>();
        int i = 0;

        foreach (var (suffix, args) in _args)
        {
            var tool = tools.FirstOrDefault(t => t.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                       ?? throw new InvalidOperationException(
                           $"No tool matches '{suffix}'. Offered: {string.Join(", ", tools.Select(t => t.Name))}");

            contents.Add(new FunctionCallContent($"call_{i++}", tool.Name,
                new Dictionary<string, object?>(args)));
        }
        return new ChatResponse(new ChatMessage(ChatRole.Assistant, contents));
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken ct = default)
        => throw new NotSupportedException();

    public object? GetService(Type serviceType, object? serviceKey = null) => null;
    public void Dispose() { }
}