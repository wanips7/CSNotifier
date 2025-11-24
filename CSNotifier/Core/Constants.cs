using System.Reflection;

namespace CSNotifier.Core;

public static class Constants
{
    public static readonly string AppVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString(3);

    public const ushort ListenerPort = 3000;

    public static readonly string SettingsFileName = AppContext.BaseDirectory + "settings.dat";

}
