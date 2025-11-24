using System.Text.Json.Serialization;

namespace CSNotifier.GameStateIntegration.Models;

public class PlayerState
{
    [JsonPropertyName("health")]
    public int Health { get; set; }

}
