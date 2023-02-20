using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public static class FiguresList
    {
        /// <summary>
        /// Класс хранящий в себе все фигуры
        /// </summary>
        public static List<Figure> ListFigures;

        public static List<Shape> ShapeList;
        static FiguresList()
        {
            ListFigures = new List<Figure>();
            ShapeList = new List<Shape>();
        }
        public static void AddFigure(Figure figure)
        {
            ListFigures.Add(figure);
        }
    }
}
