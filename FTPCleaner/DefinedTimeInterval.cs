using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPCleaner
{
    public class DefinedTimeInterval
    {
        private TimeSpan DefinedTime { get; set; }
        public DefinedTimeInterval() { init(); }
        public void init(int weeks=0, int days=0, int hours=0, int minutes=0) { DefinedTime = new TimeSpan(weeks * 7 + days, hours, minutes, 0); }
        public bool IsOldFile(DateTime file_timestamp)
        {
            TimeSpan interval = DateTime.Now - file_timestamp;
            double dif = interval.TotalMinutes - DefinedTime.TotalMinutes;
            return dif > 0;
        }
    }
}
