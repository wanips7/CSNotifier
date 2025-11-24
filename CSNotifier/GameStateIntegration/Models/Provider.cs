using System.Text.Json.Serialization;

namespace CSNotifier.GameStateIntegration.Models;

public class Provider
{
    [JsonPropertyName("steamid")]
    public UInt64 SteamId { get; set; }

}
