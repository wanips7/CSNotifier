using System.IO;
using System.Text.Json;

namespace CSNotifier.ApplicationSettings;

public static class SettingsReader
{
    public static Settings GetDefault()
    {
        return new Settings();
    }

    public static Settings ReadFromFile(string fileName)
    {
        var text = File.ReadAllText(fileName);

        return JsonSerializer.Deserialize<Settings>(text) ?? throw new NullReferenceException();
    }

    public static Settings ReadFromFileOrDefault(string fileName)
    {
        if (File.Exists(fileName))
        {
            return ReadFromFile(fileName);
        }
       
        return GetDefault();
    }

}
