using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Scanner.DbContext;
using Scanner.UI.MainMenuFolder;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
    string login, passw;
    private void Window_Closed(object? sender, EventArgs e)
    {
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.login = Properties.Settings.Default.UserLogin;
        if (this.login != "")
        {
            this.login = Properties.Settings.Default.UserLogin;
            this.passw = Properties.Settings.Default.UserPassword;

            text_login.Text = this.login;
            text_password.Password = this.passw;

            checkBox.IsChecked = true;
        }
        if (this.passw == "")
        {
            checkBox.IsChecked = false;
        }
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
            using (var db = new AppDbContext())
            {
                if (db.clock_users_list.Any(u => u.login == userLogin) )
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
                    MessageBox.Show("Invalid username or password");
            }
        }
        else
            MessageBox.Show("Pusoy polya");
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