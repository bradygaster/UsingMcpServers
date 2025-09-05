using ModelContextProtocol.Server;
using System.ComponentModel;

namespace OneMpcServer.Tools;

[McpServerToolType]
public static class LeetSpeakTool
{
    [McpServerTool, Description("Convert text to leet speak (1337 speak) format by replacing letters with numbers and symbols.")]
    public static string ToLeetSpeak(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var leetMap = new Dictionary<char, string>
        {
            {'a', "4"}, {'A', "4"},
            {'b', "8"}, {'B', "8"},
            {'e', "3"}, {'E', "3"},
            {'g', "6"}, {'G', "6"},
            {'i', "1"}, {'I', "1"},
            {'l', "1"}, {'L', "1"},
            {'o', "0"}, {'O', "0"},
            {'s', "5"}, {'S', "5"},
            {'t', "7"}, {'T', "7"},
            {'z', "2"}, {'Z', "2"}
        };

        var result = new System.Text.StringBuilder();
        
        foreach (char c in input)
        {
            if (leetMap.TryGetValue(c, out string? replacement))
            {
                result.Append(replacement);
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }
}