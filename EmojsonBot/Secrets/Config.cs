using System.Text.Json.Serialization;

namespace EmojsonBot.Secrets;

internal class Config
{
    [JsonPropertyName("botToken")]
    public string BotToken { get; set; }

    [JsonPropertyName("devId")]
    public ulong? DevId { get; set; }
}