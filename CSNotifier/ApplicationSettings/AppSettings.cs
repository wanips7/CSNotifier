namespace CSNotifier.ApplicationSettings;

public class Settings
{
    public bool ShowNotificationWhenRoundStarts { get; set { field = value; OnChange?.Invoke(); } }

    public bool RestoreGameWindowWhenRoundStarts { get; set { field = value; OnChange?.Invoke(); } }

    public bool StartListeningAfterLaunching { get; set { field = value; OnChange?.Invoke(); } }

    public event Action? OnChange;

}

public static class AppSettings
{
    public static bool IsChanged { get; set; } = false;

    public static Settings Settings 
    { 
        get;
        set 
        {
            field = value;
            field.OnChange += () => { IsChanged = true; };
        } 
    } = new(); 

}
