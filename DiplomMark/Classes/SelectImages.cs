using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiplomMark.Classes
{
    public class SelectImages
    {
        public ImageSource imageSource { get; set; }
        public string uritoFile { get; set; }
        public bool isCheck { get; set; }
        public int imageWidth { get; set; }
        public int imageHeight { get; set; }

        public BitmapImage CheckIcon
        {
            get
            {
                return (isCheck) ? ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/correct.png")) :
                    ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/remove.png"));
            }
        }
    }
}
