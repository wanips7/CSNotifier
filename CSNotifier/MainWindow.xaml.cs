using CSNotifier.Core;
using CSNotifier.GameStateIntegration;
using CSNotifier.ApplicationSettings;
using CSNotifier.Windows;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSNotifier;

public partial class MainWindow : Window
{
    private readonly SolidColorBrush _listeningColor = new(Color.FromArgb(255, 0x12, 0xBB, 0x30));

    private readonly SolidColorBrush _notListeningColor = new(Color.FromArgb(255, 0x7E, 0x7E, 0x7E));

    private readonly GameStateListener _gameStateListener;

    public MainWindow()
    {        
        InitializeComponent();

        Title = "CSNotifier " + Constants.AppVersion;

        LoadSettings();

        _gameStateListener = new GameStateListener(Constants.ListenerPort);

        _gameStateListener.OnStart += () =>
        {
            TextBlockStatus.Text = "Status: listening";
            TextBlockStatusChar.Foreground = _listeningColor;

            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = true;
        };

        _gameStateListener.OnStop += () =>
        {
            TextBlockStatus.Text = "Status: not listening";
            TextBlockStatusChar.Foreground = _notListeningColor;

            ButtonStart.IsEnabled = true;
            ButtonStop.IsEnabled = false;
        };

        _gameStateListener.GameEventsExecutor.OnRoundStart += () =>
        {
            if (GameWindowFinder.IsWindowInFocus())
                return;

            if (AppSettings.Settings.ShowNotificationWhenRoundStarts)
            {
                new ToastContentBuilder()
                   .AddText("CSNotifier")
                   .AddText("The round has begun.")
                   .Show();

            }

            if (AppSettings.Settings.RestoreGameWindowWhenRoundStarts)
            {
                GameWindowFinder.RestoreAndActivateWindow();
            }
        };


    }

    private void LoadSettings()
    {
        AppSettings.Settings = SettingsReader.ReadFromFileOrDefault(Constants.SettingsFileName);

        ShowNotificationCheckBox.IsChecked = AppSettings.Settings.ShowNotificationWhenRoundStarts;
        RestoreGameWindowCheckBox.IsChecked = AppSettings.Settings.RestoreGameWindowWhenRoundStarts;
        StartListeningAfterLaunchingCheckBox.IsChecked = AppSettings.Settings.StartListeningAfterLaunching;
    }

    private async void ButtonClickStart(object sender, RoutedEventArgs e)
    {
        await _gameStateListener.StartAsync();

    }

    private async void ButtonClickStop(object sender, RoutedEventArgs e)
    {
        await _gameStateListener.StopAsync();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var gamePath = GamePathFinder.GetGamePath();

        if (gamePath is null)
        {
            CustomMessageBox.ShowDialog("Error", "Can't find game path.");
            return;
        }

        try
        {
            var gsiPath = GamePathFinder.BuildGSIFilePath(gamePath);
            
            GamestateIntegrationBuilder.BuildFile(gsiPath);

            CustomMessageBox.ShowDialog("Information", "Gamestate integration file was copied successfully. Path: \n" +
                gsiPath);
        }
        catch (Exception ex)
        {
            CustomMessageBox.ShowDialog("Error", ex.Message);
        }

    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (AppSettings.IsChanged)
            SettingsWriter.WriteToFile(Constants.SettingsFileName, AppSettings.Settings);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        var command = "-gamestateintegration";
        
        Clipboard.SetText(command);
    }

    private void StartListeningAfterLaunchingCheckBox_Click(object sender, RoutedEventArgs e)
    {
        AppSettings.Settings.StartListeningAfterLaunching = (sender as CheckBox)!.IsChecked!.Value;
    }

    private void ShowNotificationCheckBox_Click(object sender, RoutedEventArgs e)
    {
        AppSettings.Settings.ShowNotificationWhenRoundStarts = (sender as CheckBox)!.IsChecked!.Value;
    }

    private void RestoreGameWindowCheckBox_Click(object sender, RoutedEventArgs e)
    {
        AppSettings.Settings.RestoreGameWindowWhenRoundStarts = (sender as CheckBox)!.IsChecked!.Value;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (AppSettings.Settings.StartListeningAfterLaunching)
        {
            await _gameStateListener.StartAsync();
        }
    }
}