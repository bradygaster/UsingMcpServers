# Live Demo Script - Agent Framework Evolution

## Pre-Demo Setup Checklist

### 1. Environment Setup
- [ ] Ensure Azure OpenAI resource is configured with user secrets
- [ ] MCP server is running: `cd TranslationTools && dotnet run`
- [ ] Switch to agent-framework branch: `git checkout agent-framework`
- [ ] Test base functionality with 3 MCP tools

### 2. Demo Environment
- [ ] Two terminal windows open
- [ ] Code editor with `IHostApplicationBuilderExtensions.cs` ready
- [ ] README visible for reference

## Demo Script

### Introduction (2 minutes)
**"Today I'll show you the evolution of an AI application - from basic MCP tools to Agent Framework orchestration, culminating in a hybrid architecture."**

### Checkpoint 1: Agent Framework Integration (3 minutes)

**Talking Points:**
- "Here's our translation app enhanced with Agent Framework"
- "Same MCP tools, but now with better orchestration"
- "Notice the improved conversation flow and memory"

**Demo Steps:**
1. Run: `cd TranslationAgent && dotnet run`
2. Test: "Translate 'Hello world' into all available formats"
3. Show conversation history: type `history`
4. Test: "Do it again but with 'Good morning'"
5. Highlight: Agent remembers context, better UX

**Expected Output:**
```
CodeSpeak: Ellohzx Orldwzx
LeetSpeak: H3110 w0r1d
Reverse: dlrow olleH
```

### Checkpoint 2: Live Hybrid Tool Addition (5 minutes) ⭐

**Talking Points:**
- "Now let's add a 4th tool using Agent Framework patterns"
- "This shows hybrid architecture in action"
- "MCP tools + Agent Framework tools = unified experience"

**Demo Steps:**
1. Stop the application (Ctrl+C)
2. Open `IHostApplicationBuilderExtensions.cs`
3. Find the commented PirateSpeak tool code
4. **Live uncomment these lines:**
```csharp
var pirateSpeakTool = AIFunctionFactory.Create(
    [Description("Convert text to Pirate Speak - adds 'ahoy', 'matey', replaces 'you' with 'ye'")]
    static string ToPirateSpeak(string input) => 
        input.Replace("you", "ye")
             .Replace("my", "me") 
             .Replace("hello", "ahoy")
             .Replace("hi", "ahoy") + ", matey!");
```
5. **Uncomment in tools array:**
```csharp
tools: [
    .. mcpTools.Cast<AITool>(),
    pirateSpeakTool  // Add this line
]
```
6. Save file
7. Restart: `dotnet run`
8. Test: "Translate 'Hello, how are you today?' into all available formats"

**Expected Output After Addition:**
```
CodeSpeak: Ellohzx, Owhzx Rezx Uoyzx Odayzxtay?
LeetSpeak: H3110, h0w 4r3 y0u 70d4y?
Reverse: ?yadot uoy era woh ,olleH
PirateSpeak: Ahoy, how are ye today, matey!
```

### Checkpoint 3: Architecture Explanation (3 minutes)

**Talking Points:**
- "Notice: All 4 tools work seamlessly together"
- "3 tools via MCP (external modularity)"
- "1 tool via Agent Framework (native performance)"
- "This is practical AI engineering - evolution not revolution"

**Visual Demonstration:**
1. Show the application working with all 4 tools
2. Explain the hybrid approach benefits
3. Mention real-world migration scenarios

## Key Demo Messages

### Technical Benefits
✅ **"Agent Framework adds orchestration without breaking existing tools"**
✅ **"Hybrid architecture gives you choice and flexibility"**
✅ **"Conversation memory and better error handling come for free"**

### Business Benefits
✅ **"Incremental migration path - no big bang rewrites"**
✅ **"Teams can choose the best tool pattern for their needs"**
✅ **"Future-proof architecture that grows with your requirements"**

### Developer Experience
✅ **"Simple integration - MCP tools become Agent Framework tools seamlessly"**
✅ **"Type safety and debugging for native tools"**
✅ **"Modularity and reusability for MCP tools"**

## Backup Scenarios

### If MCP Server Fails
- Keep calm, restart the TranslationTools server
- Explain: "This shows the modular nature - tools are independent"

### If Agent Framework Fails
- Fallback to explaining the architecture without live demo
- Use the README examples to show the code patterns

### If Live Coding Goes Wrong
- Pre-prepare a backup branch with PirateSpeak already uncommented
- `git checkout demo-ready` (if needed)

## Post-Demo Discussion Points

### Questions to Anticipate
**Q: "When should I use MCP vs Agent Framework tools?"**
**A:** "MCP for reusable, cross-application tools. Agent Framework for performance-critical, agent-specific functions."

**Q: "How do I migrate existing tools?"**
**A:** "You don't have to! Keep them as MCP tools and add new ones as Agent Framework tools."

**Q: "What about performance differences?"**
**A:** "Agent Framework tools are faster (no network), MCP tools are more modular. Choose based on your needs."

## Success Metrics
- [ ] Audience sees all 4 tools working together
- [ ] Live coding moment succeeds
- [ ] Architecture benefits are clear
- [ ] Questions indicate understanding

---

**Remember: This demo shows evolution, not revolution. The power is in the seamless integration and developer choice!**