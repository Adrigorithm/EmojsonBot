using System.Text.Json.Serialization;

internal record Config(
    [property: JsonPropertyName("botToken")] string BotToken,
    [property: JsonPropertyName("devId")] ulong DevId
);