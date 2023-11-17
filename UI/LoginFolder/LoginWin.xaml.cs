using System.Windows;
using System.Windows.Input;

namespace Scanner.UI.LoginFolder;

/// <summary>
/// Interaction logic for LoginWin.xaml
/// </summary>
public partial class LoginWin : Window
{
    public LoginWin()
    {
        InitializeComponent();
    }

    private void Window_Closed(object? sender, EventArgs e)
    {
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    private void ExitBtn_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void butt_Log_Click(object sender, RoutedEventArgs e)
    {
    }

    private void text_passw_hide_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
    }
}