using System.Windows;
using Scanner.UI.LoginFolder;

namespace Scanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var loginWin = new LoginWin();
            loginWin.Show();
        }
    }
}
