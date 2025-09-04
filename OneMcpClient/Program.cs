using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneMcpClient;

var builder = Host.CreateApplicationBuilder(args);

// Add user secrets (always load them for this demo application)
builder.Configuration.AddUserSecrets<Program>();

// Add the MCP Client for the MCP Server containing our tools
builder.Services.AddMcpClient();

// Add the Azure OpenAI client for the chat client that will interact with the MCP Client
builder.AddAzureOpenAi();

// Add the UI layer for the chat client that will interact with the user
builder.Services.AddSingleton<ChatClientUI>();

// Build the application
var app = builder.Build();

// Run the application
var chatClientUi = app.Services.GetRequiredService<ChatClientUI>();
await chatClientUi.RunAsync();