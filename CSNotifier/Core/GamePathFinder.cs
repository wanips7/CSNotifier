using Microsoft.Win32;
using System.IO;

namespace CSNotifier.Core;

public static class GamePathFinder
{
    public static string? GetSteamPath()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
        if (key is null)
            return null;
        
        var steamPath = key.GetValue("SteamPath") as string;

        if (!string.IsNullOrEmpty(steamPath) && Directory.Exists(steamPath))
            return steamPath;

        return null;
    }

    public static string? GetGamePath()
    {
        var steamPath = GetSteamPath();

        if (string.IsNullOrEmpty(steamPath))
            return null;

        var libPath = Path.Combine(steamPath, "steamapps\\libraryfolders.vdf");

        try
        {
            var lines = File.ReadAllLines(libPath);

            string path = "";

            foreach (var line in lines)
            {
                if (line.Contains("\"path\""))
                {
                    path = line.Split("\t", StringSplitOptions.RemoveEmptyEntries)[1];
                }
                else if (line.Contains("\"730\""))
                {
                    path = path.Replace("\\\\", "\\").Replace("\"", "");
                    path = Path.Combine(path, "steamapps\\common\\Counter-Strike Global Offensive");

                    if (Directory.Exists(path))
                    {
                        return path;
                    }

                    break;
                }
            }
        }
        catch
        {
            
        }

        return null;
    }

    public static string BuildGSIFilePath(string gamePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(gamePath);
        
        return Path.Combine(gamePath, "game\\csgo\\cfg\\gamestate_integration_csn.cfg");
    }


}
