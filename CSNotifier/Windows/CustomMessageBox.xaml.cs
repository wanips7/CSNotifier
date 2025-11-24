using System.Windows;

namespace CSNotifier.Windows;

public partial class CustomMessageBox : Window
{
    public CustomMessageBox(string title, string content)
    {
        InitializeComponent();

        Title = title;
        ContentTextBlock.Text = content;
    }

    public static void ShowDialog(string title, string content)
    {
        var dialog = new CustomMessageBox(title, content);
        dialog.ShowDialog();    
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
