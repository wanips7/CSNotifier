using System.IO;
using System.Reflection;

namespace CSNotifier.GameStateIntegration;

public static class GamestateIntegrationBuilder
{
    private static string ReadEmbeddedTextFile(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        if (stream is null)
        {
            throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }

    public static void BuildFile(string filePath)
    {
        string content = ReadEmbeddedTextFile("CSNotifier.Resources.gamestate_integration_csn.txt");

        File.WriteAllText(filePath, content);
    }

}
