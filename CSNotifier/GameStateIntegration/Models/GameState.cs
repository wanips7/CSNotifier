using System.Text.Json.Serialization;

namespace CSNotifier.GameStateIntegration.Models;

public class GameState
{
    [JsonPropertyName("provider")]
    public Provider Provider { get; set; } = new();

    [JsonPropertyName("map")]
    public Map Map { get; set; } = new();

    [JsonPropertyName("round")]
    public Round Round { get; set; } = new();

    [JsonPropertyName("player")]
    public Player Player { get; set; } = new();

}
