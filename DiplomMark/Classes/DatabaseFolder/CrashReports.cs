using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes.DatabaseFolder
{
    public class CrashReports
    {
        public int Id { get; set; }
        public string ExceptionError { get; set; }
        public DateTime ErrorDate { get; set; }
        public TimeOnly ErrorTime { get; set; }
      
    }
}
