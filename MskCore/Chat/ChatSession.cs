using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

using MskCore.Options;

namespace MskCore.Chat;

public sealed class ChatSession(Kernel kernel, IChatCompletionService chat, IOptions<PromptExecutionOptions> options, ITokenUsageMonitor tokenUsageMonitor)
{
    private readonly ChatHistory _history = new();
    private readonly OpenAIPromptExecutionSettings _settings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        Temperature = options.Value.Temperature
    };

    public async Task<string> SendAsync(string userInput, CancellationToken ct = default)
    {
        _history.AddUserMessage(userInput);
        var result = await chat.GetChatMessageContentAsync(_history, _settings, kernel, ct);

        if (result.InnerContent is OpenAI.Chat.ChatCompletion innerContent)
        {
            tokenUsageMonitor.InputTokens = innerContent.Usage.InputTokenCount;
            tokenUsageMonitor.OutputTokens = innerContent.Usage.OutputTokenCount;
            tokenUsageMonitor.TotalTokens = innerContent.Usage.TotalTokenCount;
        }
        
        _history.AddAssistantMessage(result.Content ?? string.Empty);
        return result.Content ?? string.Empty;
    }
}