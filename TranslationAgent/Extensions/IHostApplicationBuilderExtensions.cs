using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class IHostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddAzureOpenAi(this IHostApplicationBuilder builder)
    {
        var endpointValue = builder.Configuration["AZURE_OPENAI_ENDPOINT"]!;
        var modelName = builder.Configuration["MODEL_NAME"]!;
        var endpoint = new Uri(endpointValue);
        var client = new AzureOpenAIClient(endpoint, new DefaultAzureCredential());

        var chatClient =
            new ChatClientBuilder(
                client.GetChatClient(modelName).AsIChatClient())
                .UseFunctionInvocation()
                .Build();

        builder.Services.AddSingleton(chatClient);
        return builder;
    }
}
