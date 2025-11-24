using System.Runtime.InteropServices;

namespace CSNotifier.Core;

public static class GameWindowFinder
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_RESTORE = 9; 

    public const string WindowClass = "SDL_app";

    public const string WindowTitle = "Counter-Strike 2";

    public static bool TryFindWindow()
    {
        return TryFindWindow(out _);
    }

    public static bool TryFindWindow(out nint hWnd)
    {
        hWnd = FindWindow(WindowClass, WindowTitle);

        return hWnd != IntPtr.Zero;
    }

    public static bool IsWindowInFocus()
    {
        return TryFindWindow(out var h) && h == GetForegroundWindow();
    }

    public static void RestoreAndActivateWindow()
    {
        if (TryFindWindow(out var h))
        {
            ShowWindow(h, SW_RESTORE);
            SetForegroundWindow(h);
        }
    }
}
