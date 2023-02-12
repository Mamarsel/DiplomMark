using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    public class CanvasDrawer
    {
        /// <summary>
        /// Класс для отрисовки фигур внутри Canvas
        /// </summary>
        public static double
            coord_x,
            coord_y,
            width,
            height;
        public static Shape SelectedShape;
        public CanvasDrawer(Shape shape)
        {
            SelectedShape = shape;
        }
        public void DrawRectangle(Point startPoint, Point pos)
        {
            coord_x = Math.Min(pos.X, startPoint.X) - 1;
            coord_y = Math.Min(pos.Y, startPoint.Y) - 1;
            width   = Math.Max(pos.X, startPoint.X) - coord_x;
            height  = Math.Max(pos.Y, startPoint.Y) - coord_y;
            SelectedShape.Width   = width;
            SelectedShape.Height  = height;
            SelectedShape.Opacity = 0.6;
            
            Canvas.SetLeft(SelectedShape, coord_x);
            Canvas.SetTop(SelectedShape, coord_y);

        }
        public void DrawEllipse(Point startPoint, Point pos)
        {
            double minX = Math.Min(pos.X, startPoint.X);
            double minY = Math.Min(pos.Y, startPoint.Y);
            double maxX = Math.Max(pos.X, startPoint.X);
            double maxY = Math.Max(pos.Y, startPoint.Y);
            Canvas.SetTop(SelectedShape, minY);
            Canvas.SetLeft(SelectedShape, minX);
            double height = maxY - minY;
            double width = maxX - minX;
            SelectedShape.Height  = Math.Abs(height);
            SelectedShape.Width   = Math.Abs(width);
            SelectedShape.Opacity = 0.6;
        }
        

    }
}
