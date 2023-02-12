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
        ///<param name="counterPhoto">Текущее фото</param>
        ///<param name="GeneralList">Основной лист</param>
        ///<param name="paths">Все ссылки на фото</param>
        public static List<Figure> ListRectangleInPhoto(List<Figure> GeneralList, List<string> paths, int counterPhoto)
        {
            List<Figure> list = new List<Figure>();
            foreach (Figure shape in GeneralList)
            {
                if (paths.Count - 1 >= counterPhoto && counterPhoto > -1)
                {
                    if (shape.toFileName == paths[counterPhoto])
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
        /// <param name="figuresList"></param>
        /// <returns></returns>
        public static List<Shape> ListToPrintShapes(List<Figure> figuresList)
        {
            List<Shape> listResult = new List<Shape>();
            foreach (Figure shape in figuresList)
            {
                if (shape.TypeFigure == "Rectangle")
                {
                    var rect = new Rectangle
                    {
                        Fill = shape.colorFill,
                        Height = shape.shape.Height,
                        Width = shape.shape.Width,
                        Name = shape.shape.Name,
                        Stroke = shape.shape.Stroke
                    };
                    listResult.Add(rect);
                }
                else if (shape.TypeFigure == "Ellipse")
                {
                    var rect = new Ellipse
                    { 
                        Fill = shape.colorFill, 
                        Height = shape.shape.Height, 
                        Width = shape.shape.Width, 
                        Name = shape.shape.Name,
                        Stroke = shape.shape.Stroke
                    };
                   
                    listResult.Add(rect);
                }

            }
            return listResult;
        }
    }
}
