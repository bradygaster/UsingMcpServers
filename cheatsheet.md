## line 15 in AgentChatUI.cs
Console.WriteLine("🚀 Add PirateSpeak tool during demo (Agent Framework Tool)");

## adding the pirate speak tool's namespace
using TranslationAgent.Tools;

## add after line 31 in AgentChatUI.cs
// Pirate speak tool that's a AITool rather than an MCP tool
var pirateSpeakTool = PirateSpeakTool.Create();
if (pirateSpeakTool != null) allTools.Add(pirateSpeakTool);