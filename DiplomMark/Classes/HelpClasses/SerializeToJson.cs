using DiplomMark.Classes.Figures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes.HelpClasses
{
    public class SerializeToJson
    {
        /// <summary>
        /// Класс для десериализации и сериализации в JSON
        /// </summary>
        /// <param NameFigure="Json"></param>
        /// <returns></returns>
        public static List<Figure> DeserealizingJSON(string Json)
        {
            var listfigures = new List<Figure>();
            List<ShapeFigure> figures = JsonConvert.DeserializeObject<List<ShapeFigure>>(Json);
            foreach (var figure in figures)
            {
                if (figure.TypeFigure == "Rectangle")
                {
                    Rectangle shape = new Rectangle() { Fill = figure.ColorFill, Width = figure.Width, Height = figure.Height, Name = figure.NameFigure, Opacity = figure.FigureOpacity, Stroke = figure.StrokeFill };
                    ShapeFigure s = new ShapeFigure(figure.NameFigure, figure.Coord_X, figure.Coord_Y, figure.ToFileName, figure.ColorFill, shape, figure.TypeFigure, shape.Width, shape.Height, figure.FigureOpacity, shape.Stroke);
                    listfigures.Add(s);
                }
                if (figure.TypeFigure == "Ellipse")
                {
                    Ellipse shape = new Ellipse() { Fill = figure.ColorFill, Width = figure.Width, Height = figure.Height, Name = figure.NameFigure, Stroke = figure.StrokeFill };
                    ShapeFigure s = new ShapeFigure(figure.NameFigure, figure.Coord_X, figure.Coord_Y, figure.ToFileName, figure.ColorFill, shape, figure.TypeFigure, shape.Width, shape.Height, figure.FigureOpacity, shape.Stroke);
                    listfigures.Add(s);
                }
            }
            return listfigures;
        }
    }
}
