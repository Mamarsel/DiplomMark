using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public class EllipseShape : Figure
    {
        /// <summary>
        /// Класс, наследуемый от абстрактного класса Figure для прорисовки Эллипса
        /// </summary>
        public static List<EllipseShape> ellipseShapes = new List<EllipseShape>();
        public EllipseShape(string name, double x, double y, string fileName, Brush colorFill, Ellipse sh, double opacity)
        {
            this.coord_x = Math.Round(x, 4);
            this.coord_y = Math.Round(y, 4);
            this.width = Math.Round(sh.ActualWidth, 4);
            this.height = Math.Round(sh.ActualHeight, 4);
            this.name = sh.Name;
            this.toFileName = fileName;
            this.colorFill = colorFill;
            this.TypeFigure = "Ellipse";
            this.shape = sh;
            this.opacity = opacity;
        }
        public static EllipseShape EllipseToFigure(Ellipse rect, double coord_x, double coord_y, string file_name, string name, double opacity)
        {
            EllipseShape figure = new EllipseShape(name, coord_x, coord_y, file_name, rect.Fill, rect, opacity);
            return figure;
        }
    }
}
