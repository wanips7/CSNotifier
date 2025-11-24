using System.Text.Json.Serialization;

namespace CSNotifier.GameStateIntegration.Models;

public class Player
{
    [JsonPropertyName("steamid")]
    public UInt64 SteamId { get; set; }

    [JsonPropertyName("state")]
    public PlayerState State { get; set; } = new();

     
}
