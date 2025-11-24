using System.Text.Json;
using System.Text.Json.Serialization;
using CSNotifier.GameStateIntegration.Models;

namespace CSNotifier.GameStateIntegration;

public class GameStateParser
{
    private readonly JsonSerializerOptions _jsonOptions = new()
        { NumberHandling = JsonNumberHandling.AllowReadingFromString };

    public async Task<GameState?> TryParseAsync(string text)
    {
        try
        {
            return JsonSerializer.Deserialize<GameState>(text, _jsonOptions);
        }
        catch 
        { 
            return null;        
        }
    }

}
