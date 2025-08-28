using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddMcpClient(this IServiceCollection services)
    {
        services.AddTransient<IMcpClient>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            McpClientOptions mcpClientOptions = new()
            {
                ClientInfo = new()
                {
                    Name = "AspNetCoreSseClient",
                    Version = "1.0.0"
                }
            };

            var url = "http://localhost:5106/sse";

            var clientTransport = new SseClientTransport(new()
            {
                Name = "AspNetCoreSse",
                Endpoint = new Uri(url)
            }, loggerFactory);

            // Not ideal pattern but should be enough to get it working.
            var mcpClient = McpClientFactory.CreateAsync(clientTransport, mcpClientOptions, loggerFactory).GetAwaiter().GetResult();

            return mcpClient;
        });

        return services;
    }
}
