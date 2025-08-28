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
public static class ReverseTools
{
    [McpServerTool, Description("Reverse a string.")]
    public static string ReverseString(string input) => new string(input.Reverse().ToArray());
}

[McpServerToolType]
public static class CodeSpeakTools
{
    [McpServerTool, Description("Convert text to CodeSpeak format - a custom encoding where each word is transformed by moving the first letter to the end, adding 'zx', then capitalizing every other character.")]
    public static string ToCodeSpeak(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<string>();

        foreach (var word in words)
        {
            if (word.Length == 1)
            {
                result.Add(word.ToUpper() + "zx");
            }
            else
            {
                // Move first letter to end, add 'zx'
                var transformed = word.Substring(1) + word[0] + "zx";
                
                // Capitalize every other character
                var chars = transformed.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (i % 2 == 0)
                        chars[i] = char.ToUpper(chars[i]);
                    else
                        chars[i] = char.ToLower(chars[i]);
                }
                
                result.Add(new string(chars));
            }
        }

        return string.Join(" ", result);
    }
}