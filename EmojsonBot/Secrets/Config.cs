using System.Text.Json.Serialization;

internal class Config
{
    [JsonPropertyName("botToken")]
    public string BotToken { get; set; }

    [JsonPropertyName("devId")]
    public ulong? DevId { get; set; }
}