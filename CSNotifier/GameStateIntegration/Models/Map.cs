using System.Text.Json.Serialization;

namespace CSNotifier.GameStateIntegration.Models;

public class Map
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("phase")]
    public MapPhase Phase { get; set; }

}
