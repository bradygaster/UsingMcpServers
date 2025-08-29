using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AI.Foundry.Local;
using OpenAI;
using System.ClientModel;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Client;
using Microsoft.Extensions.AI;
using System.Threading.Tasks;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ModelContextProtocol.Protocol;
using System.ComponentModel;

var builder = Host.CreateApplicationBuilder(args);

// Add the MCP Client for the MCP Server containing our tools
// builder.Services.AddMcpClient();

// Add the OpenAI Chat Client for the Foundry Local Manager
var alias = "deepseek-r1-distill-qwen-7b-generic-gpu";
var manager = await FoundryLocalManager.StartModelAsync(aliasOrModelId: alias);
var model = await manager.GetModelInfoAsync(aliasOrModelId: alias);
ApiKeyCredential key = new ApiKeyCredential(manager.ApiKey);
OpenAIClient client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = manager.Endpoint
});

// var chatClient = client.GetChatClient(model?.ModelId).AsIChatClient();
var chatClient =
    new ChatClientBuilder(
        client.GetChatClient(model?.ModelId).AsIChatClient())
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
// public class ChatClientUI(IChatClient chatClient, IMcpClient mcpClient)
public class ChatClientUI(IChatClient chatClient)
{
    [Description("Reverses the input string.")]
    public string ReverseString(string input)
    {
        return new string(input.Reverse().ToArray());
    }

    public async Task<string> Chat(string prompt = "Why is the sky blue")
    {
        ChatMessage[] message = [new ChatMessage(ChatRole.User, prompt)];
        // var tools = await mcpClient.ListToolsAsync();
        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(ReverseString)],
            MaxOutputTokens = 4096
        };

        var completionUpdates = chatClient.GetStreamingResponseAsync(message, options);

        var sb = new StringBuilder();

        var modelOutput = new StringBuilder();

        await foreach (var completionUpdate in completionUpdates)
        {

            var msg = new { MessageRole = completionUpdate.Role, Content = completionUpdate.Contents?.First() };

            // Print the completionUpdate.Contents as formatted JSON for clarity
            modelOutput.Append($"{JsonSerializer.Serialize(msg, new JsonSerializerOptions { WriteIndented = true })}");

            sb.Append(completionUpdate.Text);
        }

        var output = sb.ToString();
        File.WriteAllText("output.json", modelOutput.ToString());
        return output;
    }
}