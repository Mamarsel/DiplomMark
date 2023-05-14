
using System.ComponentModel.DataAnnotations;

namespace DiplomMark.Classes.DatabaseClasses
{
    public class SettingsUser
    {
        [Key]
        public int IdSetting { get; set; }  
        public string PathToONNXFile { get; set; }
        public bool GuideViewAI { get; set; } = false;
    }
}
