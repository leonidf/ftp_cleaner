using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace FTPCleaner
{
    /// <summary>
    /// Async Operations Class
    /// </summary>
    class OperationsAsync : NotifyProperty
    {
        #region Private Members
            /// <summary>
            /// Is Running Flag
            /// </summary>
            private bool isRunning = false;
        #endregion Private Members

        #region Properties
            /// <summary>
            /// Is Running Flag
            /// </summary>
            public bool IsRunning
            {
                get { return isRunning; }
                set
                {
                    isRunning = value;
                    OnPropertyChanged("IsRunning");
                }
            }
        #endregion Properties

        #region Public Methods
            /// <summary>
            /// Retrieve FTP Directory data
            /// </summary>
            public Task<Output> GetDirListAsync(Uri uri, string username, string password)
            {
                return Task.Run(() =>
                {
                    IsRunning = true;

                    bool res = false;
                    string names = string.Empty;

                    if (uri.Scheme == Uri.UriSchemeFtp)
                    {
                        try
                        {
                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                            request.Credentials = new NetworkCredential(username, password);

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

                    IsRunning = false;
                    return (Output) new DirInfoOutput() { Result = res, FileNames = (res ? names.Split('\n') : null) };
                });
            }
            /// <summary>
            /// Retrieve FTP File Details
            /// </summary>
            public Task<Output> GetFileDetailsAsync(Uri uri, string username, string password)
            {
                return Task.Run(() =>
                {
                    IsRunning = true;

                    bool res = false;
                    string names = string.Empty;
                    DateTime modifiedTime = new DateTime();
                    try
                    {
                        Console.WriteLine("File Name \"{0}\"", uri);

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                        request.Credentials = new NetworkCredential(username, password);

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

                    IsRunning = false;
                    return (Output) new FileInfoOutput() { Result = res, FileModifiedData = modifiedTime };
                });
            }

            /// <summary>
            /// Delete FTP File
            /// </summary>
            public Task<Output> DeleteFileAsync(Uri uri, string username, string password)
            {
                return Task.Run(() =>
                {
                    IsRunning = true;

                    bool res = false;
                    string names = string.Empty;
                    try
                    {
                        Console.WriteLine("File Name \"{0}\"", uri);

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                        request.Credentials = new NetworkCredential(username, password);

                        request.Method = WebRequestMethods.Ftp.DeleteFile;

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        response.Close();

                        res = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("DeleteFileAsync error: {0}", e.Message);
                    }

                    IsRunning = false;
                    return new Output() { Result = res };
                });
            }
        #endregion Public Methods
    }
}
