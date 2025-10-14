using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Agents.AI;
using ModelContextProtocol.Client;
using System.ComponentModel;

namespace Microsoft.Extensions.Hosting;

public static class IHostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddAzureOpenAiAgent(this IHostApplicationBuilder builder)
    {
        var endpointValue = builder.Configuration["AZURE_OPENAI_ENDPOINT"]!;
        var modelName = builder.Configuration["MODEL_NAME"]!;
        var endpoint = new Uri(endpointValue);
        var client = new AzureOpenAIClient(endpoint, new DefaultAzureCredential());

        // Register the Agent Framework agent
        builder.Services.AddSingleton<AIAgent>(sp =>
        {
            // Get MCP client to retrieve existing tools
            var mcpClient = sp.GetRequiredService<IMcpClient>();
            var mcpTools = mcpClient.ListToolsAsync().GetAwaiter().GetResult();

            // TODO: Uncomment during demo to add 4th tool as Agent Framework function
            /*
            var pirateSpeakTool = AIFunctionFactory.Create(
                [Description("Convert text to Pirate Speak - adds 'ahoy', 'matey', replaces 'you' with 'ye'")]
                static string ToPirateSpeak(string input) => 
                    input.Replace("you", "ye")
                         .Replace("my", "me") 
                         .Replace("hello", "ahoy")
                         .Replace("hi", "ahoy") + ", matey!");
            */

            // Create Agent Framework agent with MCP tools (and optionally native tools)
            var chatClient = client.GetChatClient(modelName).AsIChatClient();
            var agent = chatClient.CreateAIAgent(
                name: "TranslationAgent",
                instructions: "You are a helpful assistant translator who knows a handful of languages - not just any language. Each language you know is represented by an individual tool. When users provide strings to be translated, use all of the tools you have so the user can see the string translated into all available languages. Always use ALL available tools when translating text.",
                tools: [
                    .. mcpTools.Cast<AITool>()
                    // TODO: Add during demo: , pirateSpeakTool
                ]
            );

            return agent;
        });

        return builder;
    }
}
