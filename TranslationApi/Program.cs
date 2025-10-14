using Microsoft.Agents.AI;
using TranslationApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add user secrets (always load them for this demo application)
builder.Configuration.AddUserSecrets<Program>();

// Add the MCP Client for the MCP Server containing our tools
builder.Services.AddMcpClient();

// Add the Azure OpenAI client for the Agent Framework that will interact with the MCP Client
builder.AddAzureOpenAiAgent();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Translation API endpoint
app.MapPost("/translate", async (TranslationRequest request, AIAgent agent) =>
{
    try
    {
        // Use the same agent interaction as the console app
        var response = await agent.RunAsync(request.Text);
        
        return Results.Ok(new TranslationResponse(
            OriginalText: request.Text,
            AgentResponse: response.Text ?? "No response received"
        ));
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Translation Error"
        );
    }
});

app.Run();
