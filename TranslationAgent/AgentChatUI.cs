using Microsoft.Agents.AI;
using System.Text;

namespace TranslationAgent;

// The AgentChatUI class is responsible for interacting with the Agent Framework agent and handling user interface
public class AgentChatUI(AIAgent agent)
{
    private readonly List<string> _conversationHistory = new();

    public async Task RunAsync()
    {
        Console.WriteLine("🤖 Translation Agent powered by Agent Framework!");
        Console.WriteLine("✨ Features: MCP Tools + Agent Orchestration + Conversation Memory");
        Console.WriteLine("📝 Available tools: CodeSpeak, LeetSpeak, Reverse (MCP Tools)");
        Console.WriteLine("🚀 Add PirateSpeak tool during demo (Agent Framework Tool)");
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("What would you like to translate? (Leave empty to exit, 'history' to see conversation)");
            Console.Write("[USER]> ");
            var prompt = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(prompt))
            {
                Console.WriteLine("👋 Exiting application. Thanks for using Translation Agent!");
                return;
            }

            if (prompt.ToLower() == "history")
            {
                ShowConversationHistory();
                continue;
            }

            // Perform the agent interaction
            Console.WriteLine($"🤔 Agent thinking and using tools...");
            try
            {
                var response = await ChatWithAgent(prompt);
                Console.WriteLine($"[AGENT]> {response}");
                
                // Store in conversation history
                _conversationHistory.Add($"User: {prompt}");
                _conversationHistory.Add($"Agent: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine("🔄 The agent will try again with your next request.");
            }
            Console.WriteLine();
            Console.WriteLine("─".PadRight(80, '─'));
            Console.WriteLine();
        }
    }

    private async Task<string> ChatWithAgent(string prompt)
    {
        // Use Agent Framework's RunAsync method for orchestrated conversation
        // This handles tool invocation, conversation state, and response generation
        var response = await agent.RunAsync(prompt);
        return response.Text ?? "No response received";
    }

    private void ShowConversationHistory()
    {
        Console.WriteLine();
        Console.WriteLine("📚 Conversation History:");
        Console.WriteLine("═".PadRight(50, '═'));
        
        if (_conversationHistory.Count == 0)
        {
            Console.WriteLine("No conversation history yet.");
        }
        else
        {
            foreach (var entry in _conversationHistory)
            {
                Console.WriteLine(entry);
            }
        }
        
        Console.WriteLine("═".PadRight(50, '═'));
        Console.WriteLine();
    }
}