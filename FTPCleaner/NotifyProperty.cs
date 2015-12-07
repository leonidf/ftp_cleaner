using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FTPCleaner
{
    /// <summary>
    /// NotifyProperty Class
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        #region Events

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        #endregion
    }
}
