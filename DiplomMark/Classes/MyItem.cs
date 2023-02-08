using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public class MyItem
    {
        public int Counter { get; set; }

        public string TypeFigure { get; set; }
        public string NameFigure { get; set; }
        public Brush backgroundGrid { get; set; }
        public Shape shape { get; set; }
    }
}
