using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AI.Foundry.Local;
using OpenAI;
using System.ClientModel;
using OpenAI.Chat;
using System.Text;
using ModelContextProtocol.Client;
using System.Threading.Tasks;

var builder = Host.CreateApplicationBuilder(args);

// Add the MCP Client for the MCP Server containing our tools
builder.Services.AddMcpClient();

// Add the OpenAI Chat Client for the Foundry Local Manager
var alias = "phi-3.5-mini";
var manager = await FoundryLocalManager.StartModelAsync(aliasOrModelId: alias);
var model = await manager.GetModelInfoAsync(aliasOrModelId: alias);
ApiKeyCredential key = new ApiKeyCredential(manager.ApiKey);
OpenAIClient client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = manager.Endpoint
});

var chatClient = client.GetChatClient(model?.ModelId);
builder.Services.AddSingleton<ChatClient>(chatClient);
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
public class ChatClientUI(ChatClient chatClient, IMcpClient mcpClient)
{
    public async Task<string> Chat(string prompt = "Why is the sky blue")
    {
        ChatMessage[] message = new[] { (ChatMessage)prompt };
        var tools = await mcpClient.ListToolsAsync();
        var options = new ChatCompletionOptions { MaxOutputTokenCount = 4096 };
        var completionUpdates = chatClient.CompleteChatStreaming(message, options);

        var sb = new StringBuilder();

        foreach (var completionUpdate in completionUpdates)
        {
            if (completionUpdate.ContentUpdate.Count > 0)
            {
                sb.Append(completionUpdate.ContentUpdate[0].Text);
            }
        }

        var output = sb.ToString();
        return output;
    }
}