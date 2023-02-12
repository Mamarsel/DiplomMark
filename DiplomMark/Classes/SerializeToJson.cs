using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public class SerializeToJson
    {
        /// <summary>
        /// Класс для десериализации и сериализации в JSON
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static List<Figure> DeserealizingJSON(string Json)
        {
            var listfigures = new List<Figure>();
            List<ShapeFigure> figures = JsonConvert.DeserializeObject<List<ShapeFigure>>(Json);
            foreach (var figure in figures)
            {
                if (figure.TypeFigure == "Rectangle")
                {
                    Rectangle shape = new Rectangle() { Fill = figure.colorFill, Width = figure.width, Height = figure.height, Name = figure.name, Opacity = figure.opacity, Stroke = figure.StrokeFill };
                    ShapeFigure s = new ShapeFigure(figure.name, figure.coord_x, figure.coord_y, figure.toFileName, figure.colorFill, shape, figure.TypeFigure, shape.Width, shape.Height, figure.opacity, shape.Stroke);
                    listfigures.Add(s);
                }
                if (figure.TypeFigure == "Ellipse")
                {
                    Ellipse shape = new Ellipse() { Fill = figure.colorFill, Width = figure.width, Height = figure.height, Name = figure.name, Stroke = figure.StrokeFill };
                    ShapeFigure s = new ShapeFigure(figure.name, figure.coord_x, figure.coord_y, figure.toFileName, figure.colorFill, shape, figure.TypeFigure, shape.Width, shape.Height, figure.opacity, shape.Stroke);
                    listfigures.Add(s);
                }
            }
            return listfigures;
        }
    }
}
