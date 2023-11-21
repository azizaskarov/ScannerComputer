using Scanner.Models;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Scanner.DbContext;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

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
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var db = new AppDbContext();
        var computerIp = mainMenu.GetComputerIp();
        if (computerName.Text.Length != 0)
        {
            var comp = db.clock_comp_list.First(c => c.comp_name == computerName.Text);
            if (db.clock_comp_list.Any(c => c.ip == comp.ip))
            {
                if (computerIp != null)
                {

                    comp.ip = computerIp;
                    comp.comp_name = computerName.Text;

                    Properties.Settings.Default.ComputerName = comp.comp_name;
                    db.clock_comp_list.Update(comp);
                    db.SaveChanges();
                    Close();
                    MessageBox.Show("Success");
                    SaveCurrentProccessApps(comp.id);
                    mainMenu.refresh.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (computerIp != null)
                {
                    var clock_comp = new clock_comp_list()
                    {
                        ip = computerIp,
                        comp_name = computerName.Text,

                    };

                    Properties.Settings.Default.ComputerName = clock_comp.comp_name;
                    db.clock_comp_list.Add(clock_comp);
                    db.SaveChanges();
                    Close();
                    MessageBox.Show("Success");
                    SaveCurrentProccessApps(clock_comp.id);
                    mainMenu.refresh.Visibility = Visibility.Visible;
                }
              
            }

        }
        else
        {
            MessageBox.Show("Pustoy polya");
        }
    }

   
    public async void SaveCurrentProccessApps(int id)
    {
        var db = new AppDbContext();
        Process[] processlist = Process.GetProcesses();
        foreach (Process theprocess in processlist)
        {
            if (!theprocess.Responding)
                continue;

            IntPtr hwnd = theprocess.MainWindowHandle;
            if (IsWindowVisible(hwnd))
            {
                StringBuilder title = new StringBuilder(256);
                GetWindowText(hwnd, title, 256);

                var computer =await db.clock_comp_list.FirstOrDefaultAsync(u => u.id.Equals(id));
                if (computer != null)
                {
                    var appList = new clock_app_list()
                    {
                        id_comp = id.ToString(),
                        app_name = theprocess.ProcessName,
                        id_app = theprocess.Id.ToString(),
                        app_title = title.ToString()
                    };
                    await db.clock_app_list.AddAsync(appList);
                    db.SaveChanges();
                   await  mainMenu.InsertAppList();
                }
            }
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