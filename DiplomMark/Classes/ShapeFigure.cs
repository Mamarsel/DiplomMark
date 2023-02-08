using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public class ShapeFigure : Figure
    {
        public ShapeFigure(string name, double x, double y, string fileName, Brush colorFill, Shape rect, string typeFigure, double width, double height, double opacity)
        {
            this.coord_x = Math.Round(x, 4);
            this.coord_y = Math.Round(y, 4);
            this.name = name;
            this.toFileName = fileName;
            this.colorFill = (Brush)colorFill;
            this.TypeFigure = typeFigure;
            this.shape = rect;
            this.width = width;
            this.height = height;
            this.opacity = opacity;
        }
    }
}
