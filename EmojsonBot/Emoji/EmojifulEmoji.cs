using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

internal record EmojifulEmoji
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}