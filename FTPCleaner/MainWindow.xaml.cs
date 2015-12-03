using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
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

namespace FTPCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DefinedTimeInterval FileTimeout = new DefinedTimeInterval();

        private OperationsAsync Operations = new OperationsAsync();

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
            FileTimeout.init(0, 1);
            Operations.Username = "leonidfb";
            Operations.Password = "fl67fe77";
            Operations.FtpUri   = new Uri("ftp://leonidf.byethost5.com/public_html/webcam/");

            // Retrieve FTP directory
            Output output = await Operations.GetDirListAsync(Operations.FtpUri);
            if (!output.Result)
            {
                Console.WriteLine("GetDirListAsync is failed");
                return;
            }
            Console.WriteLine("output length = {0};", ((DirInfoOutput)output).FileNames.Length);

            // Retrive File Details
            Uri FileName = new Uri(Operations.FtpUri, ((DirInfoOutput)output).FileNames[0]);
            output = await Operations.GetFileDetailsAsync(FileName);
            if (!output.Result)
            {
                Console.WriteLine("GetFileDetailsAsync is failed");
                return;
            }
            Console.WriteLine("output result = {0}; File = {1}; Data = {2}", output.Result, FileName, ((FileInfoOutput) output).FileModifiedData);

            // Check is file old
            bool result = FileTimeout.IsOldFile(((FileInfoOutput)output).FileModifiedData);

            // Delete File from FTP
            output = await Operations.DeleteFileAsync(FileName);
        }
    }
}
