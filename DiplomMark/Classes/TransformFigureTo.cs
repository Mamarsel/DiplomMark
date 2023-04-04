using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public class TransformFigureTo
    {
        /// <summary>
        /// Класс для работы с преобразованием фигур
        /// </summary>

        ///<summary>
        /// Все фигуры внутри фото
        ///</summary>
        ///<param NameFigure="counterPhoto">Текущее фото</param>
        ///<param NameFigure="GeneralList">Основной лист</param>
        ///<param NameFigure="Paths">Все ссылки на фото</param>
        public static List<Figure> ListRectangleInPhoto(List<Figure> GeneralList, List<string> paths, int counterPhoto)
        {
            List<Figure> list = new List<Figure>();
            foreach (Figure shape in GeneralList)
            {
                if (paths.Count - 1 >= counterPhoto && counterPhoto > -1)
                {
                    if (shape.ToFileName == paths[counterPhoto])
                    {
                        list.Add(shape);
                    }
                }

            }
            return list;
        }
        /// <summary>
        /// Преобразование абстрактной класса Figure в другой абстрактный класс Shape
        /// </summary>
        /// <param NameFigure="figuresList"></param>
        /// <returns></returns>
        public static List<Shape> ListToPrintShapes(List<Figure> figuresList)
        {
            List<Shape> listResult = new List<Shape>();
            foreach (Figure shape in figuresList)
            {
                var rect = new Rectangle
                {
                    Fill = shape.ColorFill,
                    Height = shape.ShapeFigure.Height,
                    Width = shape.ShapeFigure.Width,
                    Name = shape.ShapeFigure.Name,
                    Stroke = shape.ShapeFigure.Stroke
                };
                listResult.Add(rect);
            }
            return listResult;
        }
    }
}
