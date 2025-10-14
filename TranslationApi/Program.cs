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
        var agentText = response.Text ?? "No response received";
        
        // Parse the agent's response to extract individual translations
        var translations = ParseTranslations(agentText);
        
        return Results.Ok(new TranslationResponse(
            OriginalText: request.Text,
            Translations: translations,
            RawAgentResponse: agentText
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

static Dictionary<string, string> ParseTranslations(string agentResponse)
{
    var translations = new Dictionary<string, string>();
    
    // Look for patterns like "1. CodeSpeak: result" or "CodeSpeak: result"
    var lines = agentResponse.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    
    foreach (var line in lines)
    {
        var trimmedLine = line.Trim();
        
        // Try to match patterns like "1. ToolName: result" or "ToolName: result"
        if (TryExtractTranslation(trimmedLine, "CodeSpeak", out var codeSpeak))
            translations["CodeSpeak"] = codeSpeak;
        else if (TryExtractTranslation(trimmedLine, "Leet Speak", out var leetSpeak))
            translations["LeetSpeak"] = leetSpeak;
        else if (TryExtractTranslation(trimmedLine, "Reversed", out var reversed))
            translations["Reverse"] = reversed;
        else if (TryExtractTranslation(trimmedLine, "Pirate Speak", out var pirateSpeak))
            translations["PirateSpeak"] = pirateSpeak;
    }
    
    return translations;
}

static bool TryExtractTranslation(string line, string toolName, out string translation)
{
    translation = string.Empty;
    
    // Look for patterns like "1. ToolName: result" or "**ToolName**: result" or "ToolName: result"
    var patterns = new[]
    {
        $"**{toolName}**:",
        $"{toolName}:",
        $". {toolName}:"
    };
    
    foreach (var pattern in patterns)
    {
        var index = line.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
        if (index >= 0)
        {
            translation = line.Substring(index + pattern.Length).Trim();
            return !string.IsNullOrEmpty(translation);
        }
    }
    
    return false;
}

app.Run();
