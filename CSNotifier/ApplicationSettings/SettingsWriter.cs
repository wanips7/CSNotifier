using System.IO;
using System.Text.Json;

namespace CSNotifier.ApplicationSettings;

public static class SettingsWriter
{
    public static void WriteToFile(string fileName, Settings settings)
    {
        var text = JsonSerializer.Serialize(settings);

        File.WriteAllText(fileName, text);

    }

}
