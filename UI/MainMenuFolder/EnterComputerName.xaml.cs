using Scanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Scanner.DbContext;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;

namespace Scanner.UI.MainMenuFolder
{
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
                if (computerIp != null)
                {
                    var clock_comp = new clock_comp_list()
                    {
                        ip = computerIp,
                        comp_name = computerName.Text,
                    };

                    db.clock_comp_list.Add(clock_comp);
                    db.SaveChanges();
                    Close();
                    MessageBox.Show("Success");
                    SaveCurrentProccessApps(clock_comp.id);
                }
            }
            else
            {
                MessageBox.Show("Pustoy polya");
            }
        }


        public void SaveCurrentProccessApps(int id)
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

                    var computer = db.clock_comp_list.FirstOrDefault(u => u.id.Equals(id));
                    if (computer != null)
                    {
                        var appList = new clock_app_list()
                        {
                            id_comp = id.ToString(),
                            app_name = theprocess.ProcessName,
                            id_app = theprocess.Id.ToString(),
                            app_title = title.ToString()
                        };
                        db.clock_app_list.Add(appList);
                        db.SaveChanges();
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
}
