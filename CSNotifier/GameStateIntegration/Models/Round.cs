using System.Text.Json.Serialization;

namespace CSNotifier.GameStateIntegration.Models;

public class Round
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("phase")]
    public RoundPhase Phase { get; set; }

}
