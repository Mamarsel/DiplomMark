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

namespace DiplomMark.Classes
{
    public class CustomCanvas : Canvas
    {
        private Point startPoint;
        private Rectangle currentRectangle;
        public static Rectangle selectedRectangle;
        private Point currentMousePosition;
        private double _mouseDownDistanceThreshold = 2;
        private  MainPage _main;

        private static double _coordX, _coordY;

        Random r = new();
        int counter = 1;
        //static byte R;
        //static byte G;
        //static byte B;
        public CustomCanvas()
        {
            this.Background = Brushes.White;
            this.MouseLeftButtonDown += OnMouseLeftButtonDown;
            this.MouseLeftButtonUp += OnMouseLeftButtonUp;
            this.MouseMove += OnMouseMove;
            this.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GlobalVars.SelectedTag == null)
                return;
            //R = (byte)r.Next(1, 255);
            //G = (byte)r.Next(1, 255);
            //B = (byte)r.Next(1, 233);
            _main = MainPage.MainPageController;
            startPoint = e.GetPosition(this);
            currentMousePosition = startPoint;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                foreach (Figure rectangle in FiguresList.ListFigures)
                {
                    if (rectangle.ShapeFigure.IsMouseOver)
                    {
                        if (selectedRectangle != null)
                        {
                            selectedRectangle.Stroke = GlobalVars.SelectedTag.TagColor;
                            selectedRectangle.Fill = GlobalVars.SelectedTag.TagColor;
                            selectedRectangle.Fill.Opacity = 0.4;
                            selectedRectangle.StrokeThickness = 1;
                        }
                        selectedRectangle = (Rectangle)rectangle.ShapeFigure;
                        selectedRectangle.Fill = new SolidColorBrush(Color.FromArgb(125, 0, 0, 255));
                        selectedRectangle.StrokeThickness = 2;
                        break;
                    }
                }
            }
            else if (IsMouseOver && (Mouse.DirectlyOver is CustomCanvas || Mouse.DirectlyOver is Rectangle))
            {
                counter++;
                currentRectangle = new Rectangle
                {
                    Stroke = GlobalVars.SelectedTag.TagColor,
                    StrokeThickness = 2,
                    Fill = GlobalVars.SelectedTag.TagColor,
                    Opacity = 0.6,
                    Name = GlobalVars.SelectedTag.TagName
                };
                Canvas.SetLeft(currentRectangle, startPoint.X);
                Canvas.SetTop(currentRectangle, startPoint.Y);
                this.Children.Add(currentRectangle);
                //_main.AddElementToListBox(
                //    new MyItem
                //    {
                //        Counter = counter,
                //        TypeFigure = $"{currentRectangle.GetType().Name.ToUpper()} SHAPE",
                //        BackgroundGrid = currentRectangle.Fill,
                //        FigureShape = currentRectangle,
                //        NameFigure = currentRectangle.Name
                //    });
                //_main.ListBoxAllElements.SelectedIndex = _main.ListBoxAllElements.Items.Count - 1;
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
                Canvas.SetLeft(currentRectangle, _coordX);
                Canvas.SetTop(currentRectangle, _coordY);
            }
            else if (selectedRectangle != null && e.LeftButton == MouseButtonState.Pressed)
            {
                currentMousePosition = e.GetPosition(this);
                _coordX = currentMousePosition.X - startPoint.X + Canvas.GetLeft(selectedRectangle);
                _coordY = currentMousePosition.Y - startPoint.Y + Canvas.GetTop(selectedRectangle);
                Canvas.SetLeft(selectedRectangle, _coordX);
                Canvas.SetTop(selectedRectangle, _coordY);
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
                    _main.OpacitySlider.Value = currentRectangle.Opacity;
                    MainPage.MainPageController.X12.SelectedColor = GlobalVars.SelectedTag.TagColor.Color;
                    MainPage.MainPageController.PreviewColorBorder.Background = GlobalVars.SelectedTag.TagColor;
                    currentRectangle = null;
                }
                else
                {
                    this.Children.Remove(currentRectangle);
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
            if (e.Key == Key.Delete && this.Children.Contains(selectedRectangle))
            {
                this.Children.Remove(selectedRectangle);
                FiguresList.ListFigures.Remove(FiguresList.ListFigures.FirstOrDefault(x=>x.ShapeFigure == selectedRectangle));
                MainPage.MainPageController.RefreshListBox();
                selectedRectangle = null;
            }
        }

    }
}
