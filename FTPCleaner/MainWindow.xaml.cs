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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;

namespace FTPCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DefinedTimeInterval FileTimeout = new DefinedTimeInterval();

        private OperationsAsync Operations = new OperationsAsync();

        private Configuration AppConfiguration = new Configuration();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "Start":
                    StartProcess();
                    break;
            }
        }

        private async void StartProcess()
        {
            // Init params
            FileTimeout.Init(0, 1);
            AppConfiguration.Username = "leonidfb";
            AppConfiguration.Password = "fl67fe77";
            AppConfiguration.FtpUri   = new Uri("ftp://leonidf.byethost5.com/public_html/webcam/");

            // Retrieve FTP directory
            if(Operations.IsRunning)
            {
                Console.WriteLine("Process is running!");
                return;
            }

            Output output = await Operations.GetDirListAsync(AppConfiguration.FtpUri, AppConfiguration.Username, AppConfiguration.Password);
            if (!output.Result)
            {
                Console.WriteLine("GetDirListAsync is failed");
                return;
            }
            Console.WriteLine("output length = {0};", ((DirInfoOutput)output).FileNames.Length);

            // Retrive File Details
            Uri FileName = new Uri(AppConfiguration.FtpUri, ((DirInfoOutput)output).FileNames[0]);
            output = await Operations.GetFileDetailsAsync(FileName, AppConfiguration.Username, AppConfiguration.Password);
            if (!output.Result)
            {
                Console.WriteLine("GetFileDetailsAsync is failed");
                return;
            }
            Console.WriteLine("output result = {0}; File = {1}; Data = {2}", output.Result, FileName, ((FileInfoOutput) output).FileModifiedData);

            // Check is file old
            bool result = FileTimeout.IsOldFile(((FileInfoOutput)output).FileModifiedData);

            // Delete File from FTP
            output = await Operations.DeleteFileAsync(FileName, AppConfiguration.Username, AppConfiguration.Password);
        }

        private void OnApplicationMenuItem(object sender, RoutedEventArgs e)
        {
            var ApplicationMenu = sender as RibbonApplicationMenuItem;

            switch (ApplicationMenu.Name)
            {
                case "WebSite":
                    break;

                case "UpdateApp":
                    break;

                case "AppExit":
                    this.Close();
                    break;

                default:
 //                   MessageBox.Show("Wrong control is selected: " + ApplicationMenu.Name, Properties.Resources.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void OnApplicationHelpButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
