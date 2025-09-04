using ModelContextProtocol.Server;
using System.ComponentModel;

namespace OneMpcServer.Tools;

[McpServerToolType]
public static class ReverseTool
{
    [McpServerTool, Description("Reverse a string.")]
    public static string ReverseString(string input) => new string(input.Reverse().ToArray());
}