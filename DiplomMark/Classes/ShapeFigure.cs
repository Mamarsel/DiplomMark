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
        public ShapeFigure(string name, double x, double y, string fileName, Brush colorFill, Shape rect, string typeFigure, double width, double height, double opacity, Brush StrokeFill)
        {
            this.coord_x = Math.Round(x, 4);
            this.coord_y = Math.Round(y, 4);
            this.name = name;
            this.toFileName = fileName;
            this.colorFill = colorFill;
            this.TypeFigure = typeFigure;
            this.shape = rect;
            this.width = width;
            this.height = height;
            this.opacity = opacity;
            this.StrokeFill = StrokeFill;
            
        }
        public static ShapeFigure ShapeToFigure(Shape rect, double coord_x, double coord_y, string file_name, string name, double opacity, Brush StrokeFill)
        {
            ShapeFigure figure = new ShapeFigure(name, coord_x, coord_y, file_name, rect.Fill, rect, rect.GetType().Name, rect.Width, rect.Height, opacity, StrokeFill);
            return figure;
        }
    }
}
