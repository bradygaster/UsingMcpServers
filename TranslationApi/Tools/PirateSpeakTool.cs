using Microsoft.Extensions.AI;
using System.ComponentModel;

namespace TranslationApi.Tools;

/// <summary>
/// Agent Framework native tool for converting text to Pirate Speak
/// This demonstrates hybrid architecture - MCP tools + Agent Framework tools working together
/// </summary>
public static class PirateSpeakTool
{
    public static AITool Create()
    {
        static string ToPirateSpeak(string input)
        {
            return "Arrrrrr " + input.Replace("you", "ye")
                       .Replace("my", "me")
                       .Replace("hello", "ahoy")
                       .Replace("hi", "ahoy") + ", matey!";
        }

        return AIFunctionFactory.Create(ToPirateSpeak,
            "ConvertTextToPirateSpeak",
            "Convert text to Pirate Speak - adds 'ahoy', 'matey', replaces 'you' with 'ye'");
    }
}