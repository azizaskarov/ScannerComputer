using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Devices;
using Scanner.DbContext;
using Scanner.Models;

namespace Scanner.UI.MainMenuFolder;

/// <summary>
/// Interaction logic for MainMenuWin.xaml
/// </summary>
public partial class MainMenuWin : Window
{
    public MainMenuWin()
    {
        InitializeComponent();
    }



    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    static extern bool IsWindowVisible(IntPtr hWnd);

    public bool StateClosed = false;
    private void Window_Closed(object? sender, EventArgs e)
    {
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void butt_Hide_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void butt_Resize_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
        }
        else
        {
            this.MaxHeight = SystemParameters.WorkArea.Height + 10;
            this.MaxWidth = SystemParameters.WorkArea.Width + 10;
            this.WindowState = WindowState.Maximized;
        }
    }

    private void ExitBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

    private void butt_menu_Click(object sender, RoutedEventArgs e)
    {
        if (StateClosed)
        {
            var sb = this.FindResource("OpenMenu") as Storyboard;
            sb!.Begin();

        }
        else
        {
            var sb = this.FindResource("CloseMenu") as Storyboard;
            sb!.Begin();

        }
        StateClosed = !StateClosed;
    }


    public List<string> GetCurrentProccessApps()
    {
        Process[] processlist = Process.GetProcesses();
        List<string> computerInfos = new List<string>();
        foreach (Process theprocess in processlist)
        {
            if (!theprocess.Responding)
                continue;

            IntPtr hwnd = theprocess.MainWindowHandle;
            if (IsWindowVisible(hwnd))
            {
                StringBuilder title = new StringBuilder(256);
                GetWindowText(hwnd, title, 256);


                computerInfos.Add($"Dastur nomi: {theprocess.ProcessName}, ID: {theprocess.Id}, Sarlavha: {title}");
            }
        }

        return computerInfos;
    }


    public async Task InsertAppList()
    {
        while (true)
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

                    var compName = Properties.Settings.Default.ComputerName;
                    var computer = await db.clock_comp_list.FirstOrDefaultAsync(u => u.comp_name == compName);
                    if (computer != null)
                    {
                        if (!db.clock_app_history_list.Any(that => that.app_title == title.ToString()))
                        {
                            var appList = new clock_app_history_list()
                            {
                                id_comp = computer.id.ToString(),
                                app_name = theprocess.ProcessName,
                                id_app = theprocess.Id.ToString(),
                                app_title = title.ToString(),
                                open_datetime = DateTime.Now,
                                open_datetime_uni = DateTime.UtcNow.Ticks.ToString()
                            };
                            await db.clock_app_history_list.AddAsync(appList);
                            await db.SaveChangesAsync();
                        }
                    }

                }
            }

            await Task.Delay(5000);
        }
    }

    public string? GetComputerIp()
    {
        string[] localIPs = Dns.GetHostAddresses(Dns.GetHostName())
            .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
            .Select(ip => ip.ToString())
            .ToArray();

        if (localIPs.Length > 0)
        {
            return localIPs[0];
        }

        return null;
    }


    private async void ZapuskBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (Properties.Settings.Default.ComputerName.Length == 0)
        {
            var enterCompNmae = new EnterComputerName(this);
            enterCompNmae.ShowDialog();
            ListView.ItemsSource = GetCurrentProccessApps();
        }
        else
        {
            using (var db = new AppDbContext())
            {
                ListView.ItemsSource = GetCurrentProccessApps();
                var comp = await db.clock_comp_list.FirstAsync(that =>
                    that.comp_name.Equals(Properties.Settings.Default.ComputerName));

                await SaveCurrentProcessApps(comp.id);

            }

        }

    }


    public async Task SaveCurrentProcessApps(int id)
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

                var computer = await db.clock_comp_list.FirstOrDefaultAsync(u => u.id.Equals(id));

                if (computer != null)
                {
                    var appList = new clock_app_list()
                    {
                        id_comp = id.ToString(),
                        app_name = theprocess.ProcessName,
                        id_app = theprocess.Id.ToString(),
                        app_title = title.ToString()
                    };

                    //for (int i = 0; i < appList.app_title.Length; i++)
                    //{
                    //    if (appList.app_title[i].Equals('/'))
                    //    {
                    //        appList.app_title = appList.app_title.Replace("/", "{\n ");
                    //    }
                    //}


                    await db.clock_app_list.AddAsync(appList);
                    await db.SaveChangesAsync();
                }
            }
        }


        await InsertAppList();
    }

    private void Refresh_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ListView.ItemsSource = GetCurrentProccessApps();
    }
}
