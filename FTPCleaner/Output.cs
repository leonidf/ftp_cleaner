using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPCleaner
{
    public class Output
    {
        public bool Result { get; set; }
    }

    public class DirInfoOutput : Output
    {
        public string[] FileNames { get; set; }
    }

    public class FileInfoOutput : Output
    {
        public DateTime FileModifiedData { get; set; }
    }
}
