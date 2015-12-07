using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPCleaner
{
    /// <summary>
    /// Defined Time Interval Class
    /// </summary>
    public class DefinedTimeInterval
    {
        #region Members
            /// <summary>
            /// Defined Time
            /// </summary>
            private TimeSpan DefinedTime { get; set; }
        #endregion Members

        #region Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            public DefinedTimeInterval() { Init(); }
        #endregion Constructor

        #region Public Methods
            /// <summary>
            /// Initialization
            /// </summary>
            public void Init(int weeks=0, int days=0, int hours=0, int minutes=0)
            {
                DefinedTime = new TimeSpan(weeks * 7 + days, hours, minutes, 0);
            }

            /// <summary>
            /// Check Is file Old
            /// </summary>
            public bool IsOldFile(DateTime file_timestamp)
            {
                TimeSpan interval = DateTime.Now - file_timestamp;
                double dif = interval.TotalMinutes - DefinedTime.TotalMinutes;
                return dif > 0;
            }
        #endregion Public Methods
    }
}
