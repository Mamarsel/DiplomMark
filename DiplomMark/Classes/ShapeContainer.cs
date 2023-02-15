using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public static class ShapeContainer
    {
        /// <summary>
        /// Класс хранящий в себе все фигуры
        /// </summary>
        public static List<Figure> list;

        public static List<Shape> ShapeList;
        static ShapeContainer()
        {
            list = new List<Figure>();
            ShapeList = new List<Shape>();
        }
        public static void AddFigure(Figure figure)
        {
            list.Add(figure);
        }
    }
}
