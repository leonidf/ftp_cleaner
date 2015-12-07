using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPCleaner
{
    /// <summary>
    /// Configuration Class
    /// </summary>
    class Configuration : NotifyProperty
    {
        #region Private Members
            /// <summary>
            /// Username
            /// </summary>
            private string username = string.Empty;

            /// <summary>
            /// Password
            /// </summary>
            private string password = string.Empty;

            /// <summary>
            /// FTP URI
            /// </summary>
            private Uri ftp_uri;
        #endregion Private Members

        #region Properties
            public string Username
            {
                get { return username; }
                set
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
            public string Password
            {
                get { return password; }
                set
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
            public Uri FtpUri
            {
                get { return ftp_uri; }
                set
                {
                    ftp_uri = value;
                    OnPropertyChanged("FtpUri");
                }
            }
        #endregion
    }
}
