using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
    private ComputerInfo computerInfo = new();

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



    private void ZapuskBtn_OnClick(object sender, RoutedEventArgs e)
    {

        string[] localIPs = Dns.GetHostAddresses(Dns.GetHostName())
            .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
            .Select(ip => ip.ToString())
            .ToArray();

        if (localIPs.Length > 0)
        {
            textBlockIP.Text = localIPs[0];
            //textBlockIP.Text = new WebClient().DownloadString("http://icanhazip.com/");
        }
        else
        {
            textBlockIP.Text = "IP address not found";
        }
    }
}





public class ComputerInfo
{
    public string IPAddress { get; set; }
    public string MachineName { get; set; }
}