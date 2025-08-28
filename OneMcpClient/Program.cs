using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using System.ClientModel;
using System.Text;
using ModelContextProtocol.Client;
using Microsoft.Extensions.AI;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

var builder = Host.CreateApplicationBuilder(args);

// Add the MCP Client for the MCP Server containing our tools
builder.Services.AddMcpClient();

// Add the OpenAI Chat Client for OpenAI
ApiKeyCredential key = new ApiKeyCredential(Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? string.Empty);
OpenAIClient client = new OpenAIClient(key);

var chatClient =
    new ChatClientBuilder(
        client.GetChatClient("gpt-4o-mini").AsIChatClient())
        .UseFunctionInvocation()
        .Build();

builder.Services.AddSingleton<IChatClient>(chatClient);
builder.Services.AddSingleton<ChatClientUI>();

// Build the application
var app = builder.Build();

// run the application
var chatClientUi = app.Services.GetRequiredService<ChatClientUI>();
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
    var output = await chatClientUi.Chat(prompt);
    Console.Write($"[ASSISTANT]> {output}");
    Console.WriteLine("");
    Console.WriteLine("");
}


// The ChatClientUI class is responsible for interacting with the OpenAI chat client
public class ChatClientUI(IChatClient chatClient, IMcpClient mcpClient)
{
    public async Task<string> Chat(string prompt = "Why is the sky blue")
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