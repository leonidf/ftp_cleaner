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
    public class Output
    {
        public bool Result { get; set; }
        public string[] FileNames { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "Start":
                    StartProcess(new Uri("ftp://leonidf.byethost5.com/public_html/webcam"));
                    break;
            }
        }

        private async void StartProcess(Uri uri)
        {
            Username = "leonidfb";
            Password = "fl67fe77";
            
            if (uri.Scheme != Uri.UriSchemeFtp)
            {
                return;
            }

            Output output = await GetDirListAsync(uri);
            Console.WriteLine("output length = {0};", output.FileNames.Length);

        }

        private Task<Output> GetDirListAsync(Uri uri)
        {
            return Task.Run(() =>
            {
                bool res = false;
                string names = string.Empty;

                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential(Username, Password);

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);

                    names = reader.ReadToEnd().Replace("..\r", "").Replace(".\r", "").Replace("\r", "");

                    reader.Close();
                    response.Close();

                    res = true;

                }
                catch (Exception e)
                {
                    res = false;
                }

                return new Output() { Result = res, FileNames = (res ? names.Split('\n') : null) };
            });
        }
    }
}
