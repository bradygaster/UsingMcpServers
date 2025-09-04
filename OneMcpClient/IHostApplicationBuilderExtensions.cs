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
        var endpoint = new Uri(endpointValue);
        var client = new AzureOpenAIClient(endpoint, new DefaultAzureCredential());

        var chatClient =
            new ChatClientBuilder(
                client.GetChatClient("gpt-4o-mini").AsIChatClient())
                .UseFunctionInvocation()
                .Build();

        builder.Services.AddSingleton<IChatClient>(chatClient);
        return builder;
    }
}
