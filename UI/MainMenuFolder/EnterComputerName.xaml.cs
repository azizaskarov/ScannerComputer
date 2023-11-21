using Scanner.Models;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Scanner.DbContext;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Scanner.UI.MainMenuFolder;

/// <summary>
/// Interaction logic for EnterComputerName.xaml
/// </summary>
public partial class EnterComputerName : Window
{
    public EnterComputerName(MainMenuWin mainMenu)
    {
        InitializeComponent();
        Owner = mainMenu;
        this.mainMenu = mainMenu;
    }
    MainMenuWin mainMenu;

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    static extern bool IsWindowVisible(IntPtr hWnd);
    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var db = new AppDbContext();
            var computerIp = mainMenu.GetComputerIp();

            if (computerName.Text.Length != 0)
            {
                if (db.clock_comp_list.Any(that => that.comp_name == computerName.Text))
                {
                    MessageBox.Show("Already exists");
                    return;
                }
                if (computerIp != null)
                {
                    var clock_comp = new clock_comp_list()
                    {
                        ip = computerIp,
                        comp_name = computerName.Text,

                    };

                    Properties.Settings.Default.ComputerName = clock_comp.comp_name;
                    Properties.Settings.Default.Save();
                    db.clock_comp_list.Add(clock_comp);
                    db.SaveChanges();
                    Close();
                    MessageBox.Show("Success"); 
                    await mainMenu.SaveCurrentProcessApps(clock_comp.id);
                    mainMenu.refresh.Visibility = Visibility.Visible;
                }

            }
            else
            {
                MessageBox.Show("Pustoy polya");
            }
        }
        catch (Exception a)
        {
            MessageBox.Show(a.ToString());
            throw;
        }
    }


    

    private void ComputerName_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var pattern = @"^[а-яА-Яa-zA-Z0-9]+$";
        var regex = new Regex(pattern);

        if (!regex.IsMatch(e.Text))
        {
            e.Handled = true;
        }
    }
}