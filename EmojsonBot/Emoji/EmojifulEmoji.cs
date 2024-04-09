using System.Text.Json.Serialization;

internal record EmojifulEmoji(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url
);