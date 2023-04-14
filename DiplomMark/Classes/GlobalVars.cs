using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes
{
    public static class GlobalVars
    {
        public static string? SelectedCatalog { get; set; }
        public static string? PathToONNXFile { get; set; }
       
        public static bool IsWindowTagOpen = false;
        public static List<TagClass> Tags = new List<TagClass>();
        public static TagClass SelectedTag { get; set; } 
    }
}
