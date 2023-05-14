using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using DiplomMark.Pages;
using DiplomMark.Classes.Figures;
using DiplomMark.Classes.HelpClasses;

namespace DiplomMark.Classes.CustomElements
{
    public class CustomCanvas : Canvas
    {
        private Point startPoint;
        private Rectangle currentRectangle;
        public static Rectangle selectedRectangle;
        private Point currentMousePosition;
        private double _mouseDownDistanceThreshold = 2;
        private MainPage _main;
        private static double _coordX, _coordY;
        Random r = new();
        int counter = 1;
        //static byte R;
        //static byte G;
        //static byte B;
        public CustomCanvas()
        {
            Background = Brushes.White;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
            MouseMove += OnMouseMove;
            PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            PreviewKeyDown += MainWindow_PreviewKeyDown;
        }
        private void _checkSelectRect()
        {
            if (selectedRectangle != null)
            {
                selectedRectangle.Stroke = GlobalVars.Tags.FirstOrDefault(x => x.TagName == selectedRectangle.Name).TagColor;
                selectedRectangle.Fill = GlobalVars.Tags.FirstOrDefault(x => x.TagName == selectedRectangle.Name).TagColor;
                selectedRectangle.Fill.Opacity = 0.7;
                selectedRectangle.StrokeThickness = 1;
                selectedRectangle = null;
            }
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GlobalVars.SelectedTag == null)
                return;
            _main = MainPage.MainPageController;
            startPoint = e.GetPosition(this);
            currentMousePosition = startPoint;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                foreach (Figure rectangle in FiguresList.ListFigures)
                {
                    if (rectangle.ShapeFigure.IsMouseOver)
                    {
                        _checkSelectRect();
                        selectedRectangle = (Rectangle)rectangle.ShapeFigure;
                        selectedRectangle.Fill = new SolidColorBrush(Color.FromArgb(125, 0, 0, 255));
                        selectedRectangle.StrokeThickness = 2;
                        break;
                    }
                }
            }
            else if (IsMouseOver && (Mouse.DirectlyOver is CustomCanvas || Mouse.DirectlyOver is Rectangle))
            {
                _checkSelectRect();
                counter++;
                currentRectangle = new Rectangle
                {
                    Stroke = GlobalVars.SelectedTag.TagColor,
                    StrokeThickness = 2,
                    Fill = GlobalVars.SelectedTag.TagColor,
                    Opacity = 0.7,
                    Name = GlobalVars.SelectedTag.TagName
                };
                SetLeft(currentRectangle, startPoint.X);
                SetTop(currentRectangle, startPoint.Y);
                Children.Add(currentRectangle);
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (currentRectangle != null && e.LeftButton == MouseButtonState.Pressed)
            {
                currentMousePosition = e.GetPosition(this);
                _coordX = Math.Min(startPoint.X, currentMousePosition.X);
                _coordY = Math.Min(startPoint.Y, currentMousePosition.Y);
                currentRectangle.Width = Math.Abs(currentMousePosition.X - startPoint.X);
                currentRectangle.Height = Math.Abs(currentMousePosition.Y - startPoint.Y);
                SetLeft(currentRectangle, _coordX);
                SetTop(currentRectangle, _coordY);
            }
            else if (selectedRectangle != null && e.LeftButton == MouseButtonState.Pressed)
            {
                currentMousePosition = e.GetPosition(this);
                _coordX = currentMousePosition.X - startPoint.X + GetLeft(selectedRectangle);
                _coordY = currentMousePosition.Y - startPoint.Y + GetTop(selectedRectangle);

                var x = FiguresList.ListFigures.FirstOrDefault(x => x.ShapeFigure == selectedRectangle);
                x.Coord_X = _coordX;
                x.Coord_Y = _coordY;

                SetLeft(selectedRectangle, _coordX);
                SetTop(selectedRectangle, _coordY);
                startPoint = currentMousePosition;
            }

        }
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentRectangle != null)
            {
                if (currentRectangle.Width > _mouseDownDistanceThreshold || currentRectangle.Height > _mouseDownDistanceThreshold)
                {
                    FiguresList.AddFigure(ShapeFigure.ShapeToFigure(currentRectangle, Math.Round(_coordX, 4), Math.Round(_coordY, 4), MainPage.Paths[MainPage.CounterImage - 1], currentRectangle.Name, currentRectangle.Opacity, currentRectangle.Stroke));
                    currentRectangle = null;
                }
                else
                {
                    Children.Remove(currentRectangle);
                }
            }
        }
        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedRectangle != null)
            {
                if (Keyboard.Modifiers != ModifierKeys.Control)
                {
                    selectedRectangle.Fill = new SolidColorBrush(Color.FromArgb(40, 0, 0, 255));
                    selectedRectangle.StrokeThickness = 1;
                    selectedRectangle = null;
                }
            }
        }
        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && Children.Contains(selectedRectangle))
            {
                Children.Remove(selectedRectangle);
                FiguresList.ListFigures.Remove(FiguresList.ListFigures.FirstOrDefault(x => x.ShapeFigure == selectedRectangle));
                MainPage.MainPageController.RefreshListBox();
                selectedRectangle = null;
            }
        }

    }
}
