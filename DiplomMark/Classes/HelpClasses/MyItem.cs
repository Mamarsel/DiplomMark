using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes.HelpClasses
{
    public class MyItem
    {
        public int Counter { get; set; }

        public string TypeFigure { get; set; }
        public string NameFigure { get; set; }
        public Brush BackgroundGrid { get; set; }
        public Shape FigureShape { get; set; }
    }
}
