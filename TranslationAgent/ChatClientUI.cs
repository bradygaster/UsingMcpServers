using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using System.Text;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace TranslationAgent;

// The ChatClientUI class is responsible for interacting with the OpenAI chat client and handling user interface
public class ChatClientUI(IChatClient chatClient, IMcpClient mcpClient)
{
    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine("What is your prompt? (Leave empty to exit)");
            Console.Write("[USER]> ");
            var prompt = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(prompt))
            {
                Console.WriteLine("Exiting application.");
                return;
            }

            // perform the chat request
            Console.WriteLine($"🕛 thinking...");
            try
            {
                var output = await Chat(prompt);
                Console.Write($"[ASSISTANT]> {output}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }

    private async Task<string> Chat(string prompt)
    {
        ChatMessage[] message = [
            new ChatMessage(ChatRole.System, "You are a helpful assistant that uses tools to answer user questions. If you need to perform a task, use the appropriate tool."),
            new ChatMessage(ChatRole.User, prompt)
        ];
        var tools = await mcpClient.ListToolsAsync();
        var options = new ChatOptions
        {
            Tools = [.. tools],
            MaxOutputTokens = 4096
        };

        var completionUpdates = chatClient.GetStreamingResponseAsync(message, options);

        var sb = new StringBuilder();

        await foreach (var completionUpdate in completionUpdates)
        {
            if (completionUpdate.Role == ChatRole.Tool)
            {
                Console.WriteLine("Called tool...");
            }
            sb.Append(completionUpdate.Text);
        }

        var output = sb.ToString();
        return output;
    }
}