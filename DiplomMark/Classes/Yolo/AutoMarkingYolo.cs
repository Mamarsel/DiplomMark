using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using DiplomMark.Pages;
using Image = System.Drawing.Image;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace DiplomMark.Classes.Yolo
{
    internal class AutoMarkingYolo
    {
        Random _rand = new();
        static byte _R;
        static byte _G;
        static byte _B;
        public void AutoMark(string urlToFile)
        {
            _R = (byte)_rand.Next(1, 255);
            _G = (byte)_rand.Next(1, 255);
            _B = (byte)_rand.Next(1, 233);
            using var yolo = new Classes.Yolo.Models.Yolov8Net(Directory.GetCurrentDirectory() + @"\Assets\Weights\yolov8m.onnx");
            Image img = Image.FromFile(urlToFile);
            using var image = Image.FromFile(urlToFile);
            var predictions = yolo.Predict(img);
            foreach (var pred in predictions)
            {
                
                var originalImageHeight = img.Height;
                var originalImageWidth = img.Width;
                var x = Math.Max(pred.Rectangle.X, 0);
                var y = Math.Max(pred.Rectangle.Y, 0);
                var width = Math.Min(originalImageWidth - x, pred.Rectangle.Width);
                var height = Math.Min(originalImageHeight - y, pred.Rectangle.Height);
                var name = pred.Label.Name.Replace(" ", "_");
                var ShapeCurrent = new Rectangle();
                Canvas.SetLeft(ShapeCurrent, x);
                Canvas.SetTop(ShapeCurrent, y);
                ShapeCurrent.Width = width;
                ShapeCurrent.Height = height;
                ShapeCurrent.StrokeThickness = 2;
                ShapeCurrent.Stroke = new SolidColorBrush(Color.FromRgb(_R, _G, _B));
                ShapeCurrent.Fill = new SolidColorBrush(Color.FromArgb(40, _R, _B, _G));
                ShapeCurrent.Name = name;
                Canvas.SetLeft(ShapeCurrent, x);
                Canvas.SetTop(ShapeCurrent, y);
                FiguresList.AddFigure(ShapeFigure.ShapeToFigure(ShapeCurrent, Math.Round(x, 4), Math.Round(y, 4), urlToFile, ShapeCurrent.Name, ShapeCurrent.Opacity, ShapeCurrent.Stroke));
                MainPage.MainPageController.OpacitySlider.Value = ShapeCurrent.Opacity;
                MainPage.MainPageController.RefreshListBox();
                MainPage.MainPageController.Cnv.Children.Add(ShapeCurrent);

            }
        }
    }
}
