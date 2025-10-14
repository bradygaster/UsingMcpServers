# Translation Agent - Agent Framework Evolution Demo

This repository demonstrates the evolution of a translation application from a basic MCP-based implementation to an enhanced version using Microsoft's Agent Framework, showcasing a hybrid architecture with both MCP tools and Agent Framework tools.

## Architecture Evolution Story

### Phase 1: Pure MCP Tools (main branch)
- 3 translation tools via MCP server (CodeSpeak, LeetSpeak, Reverse)
- Basic chat client interaction
- Manual conversation management
- Limited error handling

### Phase 2: Agent Framework Orchestration (agent-framework branch)
- Same 3 MCP tools preserved
- Enhanced with Agent Framework agent management
- Better conversation flow and error handling
- Conversation history and memory
- Improved streaming responses

### Phase 3: Hybrid Tool Architecture (Live Demo Feature)
- 3 MCP tools (CodeSpeak, LeetSpeak, Reverse)
- 1 Agent Framework tool (PirateSpeak) - added during demo
- Unified agent experience showing best of both worlds

## Key Benefits of Agent Framework Integration

### Technical Improvements
- **Conversation Memory**: Agent maintains context across interactions
- **Better Error Handling**: Robust retry logic and graceful failure recovery
- **Streaming Support**: Real-time response generation with progress feedback
- **Tool Orchestration**: Intelligent tool selection and result synthesis
- **Observability**: Built-in telemetry and monitoring capabilities

### Architecture Benefits
- **Hybrid Compatibility**: MCP tools work seamlessly with Agent Framework tools
- **Incremental Migration**: Add new capabilities without breaking existing functionality
- **Developer Choice**: Use the best tool architecture for each specific need
- **Future-Proof**: Easy evolution path as the platform advances

## Live Demo Instructions

### Setup
1. Ensure MCP server is running: `cd TranslationTools && dotnet run`
2. Switch to agent-framework branch: `git checkout agent-framework`
3. Configure Azure OpenAI settings in user secrets
4. Run: `cd TranslationAgent && dotnet run`

### Demo Flow

**Demo Checkpoint 1: "Current Agent Framework Integration"**
- Show 3 MCP tools working with Agent Framework
- Test: "Translate 'Hello world' into all available formats"
- Highlight: conversation memory, better UX, streaming responses

**Demo Checkpoint 2: "Live Hybrid Tool Addition"** ⭐
1. Stop the application
2. In `IHostApplicationBuilderExtensions.cs`, uncomment the PirateSpeak tool:
```csharp
// Uncomment these lines:
var pirateSpeakTool = AIFunctionFactory.Create(
    [Description("Convert text to Pirate Speak - adds 'ahoy', 'matey', replaces 'you' with 'ye'")]
    static string ToPirateSpeak(string input) => 
        input.Replace("you", "ye")
             .Replace("my", "me") 
             .Replace("hello", "ahoy")
             .Replace("hi", "ahoy") + ", matey!");
```
3. And add to tools array:
```csharp
tools: [
    .. mcpTools.Cast<AITool>(),
    pirateSpeakTool  // Add this line
]
```
4. Restart application
5. Test: "Translate 'Hello, how are you today?' into all available formats"
6. Observe all 4 tools working together seamlessly!

**Expected Result After Demo Addition:**
- **CodeSpeak**: "Ellohzx, Owhzx Rezx Uoyzx Odayzxtay?"
- **LeetSpeak**: "H3110, h0w 4r3 y0u 70d4y?"
- **Reverse**: "?yadot uoy era woh ,olleH"
- **PirateSpeak**: "Ahoy, how are ye today, matey!"

## Architecture Comparison

### MCP Tools (External)
- ✅ **Modular**: Independent deployment and lifecycle
- ✅ **Reusable**: Can be used by multiple agents/applications
- ✅ **Language Agnostic**: Server can be written in any language
- ⚠️ **Network Overhead**: Slight latency due to client-server communication
- ⚠️ **Deployment**: Requires separate server process

### Agent Framework Tools (Native)
- ✅ **Performance**: Direct function calls, no network overhead
- ✅ **Type Safety**: Compile-time checking and IntelliSense support
- ✅ **Debugging**: Integrated debugging experience
- ✅ **Deployment**: Single application deployment
- ⚠️ **Coupling**: Tightly coupled to the agent application

### Hybrid Approach (Best of Both)
- ✅ **Flexibility**: Choose the right tool type for each use case
- ✅ **Migration Path**: Gradual evolution without breaking changes
- ✅ **Team Enablement**: Different teams can contribute using preferred approaches
- ✅ **Real-World Ready**: Handles legacy integration scenarios

## Technology Stack

### Core Frameworks
- **Microsoft Agent Framework**: AI agent orchestration and management
- **Model Context Protocol (MCP)**: Tool integration and communication
- **Azure OpenAI**: Large language model services
- **.NET 9**: Application platform

### Key Packages
- `Microsoft.Agents.AI.OpenAI`: Agent Framework integration
- `ModelContextProtocol`: MCP client/server functionality
- `Azure.AI.OpenAI`: Azure OpenAI client
- `Microsoft.Extensions.AI`: AI abstractions and utilities

## Getting Started

### Prerequisites
- .NET 9 SDK
- Azure OpenAI service with deployed model
- Azure CLI authenticated (`az login`)

### Configuration
Set up user secrets:
```bash
cd TranslationAgent
dotnet user-secrets set "AZURE_OPENAI_ENDPOINT" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "MODEL_NAME" "gpt-4o-mini"
```

### Running the Application
```bash
# Terminal 1: Start MCP server
cd TranslationTools
dotnet run

# Terminal 2: Start Agent application
cd TranslationAgent
dotnet run
```

## Demo Talking Points

### "Evolution Not Revolution"
- "Here's how we can enhance existing MCP tools with Agent Framework"
- "Notice how our tools stay the same - we're just adding orchestration"
- "This shows Microsoft's commitment to backward compatibility"

### "Hybrid Architecture Benefits"
- "MCP for reusable, modular tools"
- "Agent Framework for conversation management and orchestration"
- "Best of both worlds - choose the right tool for the job"

### "Real-World Integration"
- "This is how you'd actually migrate existing systems"
- "Add new capabilities while preserving existing investments"
- "Demonstrates practical AI engineering approaches"

## Next Steps

- **Multi-Agent Workflows**: Create specialized agents for different translation domains
- **Workflow Orchestration**: Use Agent Framework's graph-based workflow engine
- **Enhanced Observability**: Add comprehensive telemetry and monitoring
- **Conversation Persistence**: Implement database-backed conversation storage
- **Additional Tool Types**: Explore other MCP server integrations

---

This demo showcases the power of Microsoft's unified AI platform, demonstrating how developers can evolve their applications incrementally while leveraging both established patterns (MCP) and cutting-edge capabilities (Agent Framework).