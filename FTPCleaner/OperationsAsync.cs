using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace FTPCleaner
{
    class OperationsAsync
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Uri FtpUri { get; set; }

        public Task<DirInfoOutput> GetDirListAsync(Uri uri)
        {
            return Task.Run(() =>
            {
                bool res = false;
                string names = string.Empty;

                if (uri.Scheme == Uri.UriSchemeFtp)
                {
                    try
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                        request.Credentials = new NetworkCredential(Username, Password);

                        request.Method = WebRequestMethods.Ftp.ListDirectory;

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        names = reader.ReadToEnd().Replace("..\r\n", "").Replace(".\r\n", "").Replace("..\r", "").Replace(".\r", "").Replace("\r", "");

                        reader.Close();
                        response.Close();

                        res = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("GetDirListAsync error: {0}", e.Message);
                    }
                }

                return new DirInfoOutput() { Result = res, FileNames = (res ? names.Split('\n') : null) };
            });
        }
        public Task<FileInfoOutput> GetFileDetailsAsync(Uri uri)
        {
            return Task.Run(() =>
            {
                bool res = false;
                string names = string.Empty;
                DateTime modifiedTime = new DateTime();
                try
                {
                    Console.WriteLine("File Name \"{0}\"", uri);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                    request.Credentials = new NetworkCredential(Username, Password);

                    request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    modifiedTime = response.LastModified;
                    response.Close();

                    res = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("GetFileDetailsAsync error: {0}", e.Message);
                }

                return new FileInfoOutput() { Result = res, FileModifiedData = modifiedTime };
            });
        }
        public Task<Output> DeleteFileAsync(Uri uri)
        {
            return Task.Run(() =>
            {
                bool res = false;
                string names = string.Empty;
                try
                {
                    Console.WriteLine("File Name \"{0}\"", uri);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                    request.Credentials = new NetworkCredential(Username, Password);

                    request.Method = WebRequestMethods.Ftp.DeleteFile;

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();

                    res = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("DeleteFileAsync error: {0}", e.Message);
                }

                return new Output() { Result = res };
            });
        }
    }
}
