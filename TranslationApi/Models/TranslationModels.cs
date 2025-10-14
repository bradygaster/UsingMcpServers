namespace TranslationApi.Models;

public record TranslationRequest(string Text);

public record TranslationResponse(
    string OriginalText,
    Dictionary<string, string> Translations,
    string? RawAgentResponse = null
);