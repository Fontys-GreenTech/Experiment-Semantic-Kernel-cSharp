using System.Threading;
using System.Threading.Tasks;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace MskCore.Chat;

public sealed class ChatSession(Kernel kernel, IChatCompletionService chat)
{
    private readonly ChatHistory _history = new();
    private readonly OpenAIPromptExecutionSettings _settings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    public async Task<string> SendAsync(string userInput, CancellationToken ct = default)
    {
        _history.AddUserMessage(userInput);
        var result = await chat.GetChatMessageContentAsync(_history, _settings, kernel, ct);
        _history.AddAssistantMessage(result.Content ?? string.Empty);
        return result.Content ?? string.Empty;
    }
}