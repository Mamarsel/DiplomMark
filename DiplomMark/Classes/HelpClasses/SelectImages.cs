using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiplomMark.Classes.HelpClasses
{
    public class SelectImages
    {
        public ImageSource SourceImage { get; set; }
        public string URIToFile { get; set; }
        public bool IsCheck { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }


    }
}
