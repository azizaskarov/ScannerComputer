using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Scanner.UI.MainMenuFolder;

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
        var userLogin = text_login.Text;
        string userPassword = text_password.Password;

        if (userLogin != "" && userPassword != "")
        {
            if (checkBox.IsChecked == true)
            {
                Properties.Settings.Default.UserLogin = text_login.Text;
                Properties.Settings.Default.UserPassword = text_password.Password;
                Properties.Settings.Default.Save();
            }

            var mainWin = new MainMenuWin();
            mainWin.Show();
            this.Hide();
        }
        else
        {
            MessageBox.Show("Pusoy polya");
        }
    }

    private void text_passw_hide_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        string pattern = @"^[ЁёА-яa-zA-Z0-9]+$";

        Regex regex = new Regex(pattern);

        if (!regex.IsMatch(e.Text))
        {
            e.Handled = true;
        }
    }

}