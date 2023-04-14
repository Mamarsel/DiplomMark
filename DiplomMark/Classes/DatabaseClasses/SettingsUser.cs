using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes.DatabaseClasses
{
    public class SettingsUser
    {
        [Key]
        public int IdSetting { get; set; }  
        public string PathToONNXFile { get; set; }
    }
}
