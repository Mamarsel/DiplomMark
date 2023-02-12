using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace DiplomMark.Classes
{
    public class RectangleShape : Figure
    {
        public static List<RectangleShape> rectangleShapesList = new List<RectangleShape>();

        public RectangleShape(string name, double x, double y, string fileName, Brush colorFill, Shape rect, double opacity, Brush StrokeFill)
        {
            this.coord_x = Math.Round(x, 4);
            this.coord_y = Math.Round(y, 4);
            this.width   = Math.Round(rect.Width, 4);
            this.height  = Math.Round(rect.Height, 4);
            this.name    = rect.Name;
            this.toFileName = fileName;
            this.colorFill = (Brush)colorFill;
            this.TypeFigure = "Rectangle";
            this.shape = rect;
            this.opacity = opacity;
            this.StrokeFill = StrokeFill;
        }
        public static RectangleShape RectangleToFigure(Rectangle rect, double coord_x, double coord_y, string file_name, string name, double opacity, Brush StrokeFill)
        {
            RectangleShape figure = new RectangleShape(name, coord_x, coord_y, file_name, rect.Fill, rect, opacity, StrokeFill);
            return figure;
        }

    }
}
