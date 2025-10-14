namespace TranslationApi.Models;

public record TranslationRequest(string Text);

public record TranslationResponse(
    string OriginalText,
    string AgentResponse
);