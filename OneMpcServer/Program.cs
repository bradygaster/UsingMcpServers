using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddMcpServer()
       .WithHttpTransport()
       .WithToolsFromAssembly();

var app = builder.Build();

app.MapMcp();

app.Run();

[McpServerToolType]
public static class CookPattyTool
{
    [McpServerTool, Description("Reverse a string.")]
    public static string ReverseString(string input) => new string(input.Reverse().ToArray());}