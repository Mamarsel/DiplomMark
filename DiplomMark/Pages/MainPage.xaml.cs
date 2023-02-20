﻿
using DiplomMark.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Figure = DiplomMark.Classes.Figure;
using Image = System.Drawing.Image;

using Color = System.Windows.Media.Color;

using Rectangle = System.Windows.Shapes.Rectangle;
using ColorConverter = System.Windows.Media.ColorConverter;
using Point = System.Windows.Point;
using DiplomMark.Classes.DatabaseFolder;
using System.Drawing;
using System.Windows.Controls.Primitives;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static MainPage mainPage;
        public static int counterImage        = 1;
        public static RoutedCommand MyCommand = new();
        public static int GroupIDCounter      = 1;
        public static List<Figure> rectangleShapesInPhoto = new();
        public static List<string> paths = new();
        private static List<SelectImages> _images = new();
        private Point _startPoint;
        public Shape ShapeCurrent = new Rectangle();
        int counter = 0;
        int currentPageIndex = 0;
        int itemPerPage = 10;
        int totalPage = 0;
        int totalCount = 0;
        Random r = new();

        static byte R;
        static byte G;
        static byte B;
        List<BitmapImage> itemsList = new();
        
        ApplicationContext db = new();
       
        static ListBoxItem currentSelectedListBoxItem;
       

        public MainPage(List<SelectImages> images)
        {
            InitializeComponent();
            CanvasDrawer.SelectedShape = ShapeCurrent;
            Cnv.Focus();
            Cnv.Focusable = true;

            _images = images;
            MyCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            #region Пагинирование
            AddPaths(_images);
            ImagePreview.Source = ImageController.FromFile(paths[counterImage]);
            MaxPhotosLabel.Content = paths.Count;
            totalCount = paths.Count;
            totalPage = totalCount / itemPerPage;
            if (totalCount % itemPerPage != 0)
            {
                totalPage += 1;
            }
            foreach (var fi in _images)
            {
                itemsList.Add(new BitmapImage(new Uri(fi.uritoFile)));
            }
            for (int i = 0; i < itemsList.ToList().Take(10).Count(); i++)
            {
                Thumbnails.Items.Add(itemsList[i].UriSource.AbsolutePath);
            }
            Thumbnails.SelectedIndex = 0;
            #endregion
            OpenToSave();
            mainPage = this;
        }
        /// <summary>
        /// Восстановление сохранения при открытии
        /// </summary>
        private void OpenToSave()
        {
            String sPath_SubDirectory = SearchPage.catalog + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
            {
                return;
            }
            if (File.Exists(sPath_SubDirectory + "\\saves.json"))
            {
                string text = File.ReadAllText(sPath_SubDirectory + "\\saves.json");
                var x = SerializeToJson.DeserealizingJSON(text);
                ShapeContainer.list.AddRange(x);
                rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage - 1);
                var listrect = TransformFigureTo.ListToPrintShapes(rectangleShapesInPhoto);
                PrintImagesInPhoto(rectangleShapesInPhoto, listrect);
                RefreshListBox();
            }
        }
        /// <summary>
        /// Добавление ссылок фотографии в List
        /// </summary>
        /// <param name="images"></param>
        private void AddPaths(List<SelectImages> images)
        {
            foreach (var fi in images)
            {
                paths.Add(fi.uritoFile);
            }
        }
        /// <summary>
        /// Пагинация Листбокса
        /// </summary>
        /// <param name="flag">true = Пагинация на страницу вперед
        ///                    false = Назад
        /// </param>
        private void PaginationListbox(bool flag)
        {
            try
            {
                if (!flag && currentPageIndex > 0)
                {
                    Thumbnails.Items.Clear();
                    currentPageIndex--;

                    for (int i = (currentPageIndex) * 10; i < (currentPageIndex + 1) * 10; i++)
                    {
                        Thumbnails.Items.Add(itemsList[i].UriSource);
                    }
                }
                if (flag && currentPageIndex < totalPage - 1)
                {
                    Thumbnails.Items.Clear();

                    currentPageIndex++;
                    for (int i = currentPageIndex * 10; i < (currentPageIndex + 1) * 10; i++)
                    {
                        Thumbnails.Items.Add(itemsList[i].UriSource);
                    }
                }
            }
            catch { }

        }
        /// <summary>
        /// Вывод фотографий на Canvas при переключении фото
        /// </summary>
        /// <param name="rectangleShapesInPhoto"></param>
        /// <param name="listrect"></param>
        public void PrintImagesInPhoto(List<Figure> rectangleShapesInPhoto, List<Shape> listrect)
        {
            foreach (var shapes in listrect)
            {
                foreach (var figures in rectangleShapesInPhoto)
                {
                    if (shapes.Width == figures.shape.Width && shapes.Height == figures.shape.Height && shapes.Name == figures.shape.Name)
                    {
                        Canvas.SetLeft(shapes, figures.coord_x);
                        Canvas.SetTop(shapes, figures.coord_y);
                        shapes.Opacity = figures.opacity;
                        shapes.Width = figures.shape.Width;
                        shapes.Height = figures.shape.Height;
                        shapes.Name = figures.shape.Name;
                        shapes.Fill = figures.colorFill;
                        shapes.Stroke = figures.StrokeFill;
                        shapes.StrokeThickness = figures.shape.StrokeThickness;
                        figures.shape = shapes;
                        Cnv.Children.Add(shapes);
                        TextBlock txtBox = new TextBlock() {  Width = 80, Text = shapes.Name, FontSize = 10, Name = shapes.Name };
                        Cnv.Children.Add(txtBox);
                        Canvas.SetLeft(txtBox, figures.coord_x);
                        Canvas.SetTop(txtBox, figures.coord_y);
                    }
                }
            }
        }

        #region Перелистывание фоток
        private void LabelNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (counterImage < Convert.ToInt32(MaxPhotosLabel.Content))
            {
                counterImage++;
                Cnv.Children.Clear();
                rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(rectangleShapesInPhoto);
                PrintImagesInPhoto(rectangleShapesInPhoto, listrect);
                Thumbnails.SelectedIndex = (counterImage < 10) ? counterImage - 1 : (counterImage - 1) % 10;
                if ((counterImage - 1) % 10 == 0)
                {
                    PaginationListbox(true);
                    Thumbnails.SelectedIndex = 0;
                }
                CounterLabel.Content = counterImage;
                ImagePreview.Source = ImageController.correctImage(paths, counterImage-1);
            }
        }
        private void BackLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (counterImage > 1)
            {
                counterImage--;
                Cnv.Children.Clear();
                rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(rectangleShapesInPhoto);
                PrintImagesInPhoto(rectangleShapesInPhoto, listrect);
                Thumbnails.SelectedIndex = (counterImage < 10) ? counterImage - 1 : (counterImage - 1) % 10;
                if ((counterImage - 1) % 10 == 9){

                    PaginationListbox(false);
                    Thumbnails.SelectedIndex = 9;
                }
                CounterLabel.Content = counterImage;
                ImagePreview.Source =  ImageController.correctImage(paths, counterImage - 1);
            }
        }
        private void LVBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PaginationListbox(false);
        }
        private void LvNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PaginationListbox(true);
        }
        #endregion
        /// <summary>
        /// При изменении выделенного ListBox справа автоматически устанавливаются значения NameFigure и Slider 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                currentSelectedListBoxItem = ListBoxAllElements.ItemContainerGenerator.ContainerFromIndex(ListBoxAllElements.SelectedIndex) as ListBoxItem;
                if (currentSelectedListBoxItem != null)
                {
                    string colorValue = ((SolidColorBrush)currentSelectedListBoxItem.Background).Color.ToString();
                    X12.SelectedColor = (Color)ColorConverter.ConvertFromString(colorValue);
                    MyItem myItem = (MyItem)currentSelectedListBoxItem.DataContext;
                    NameFigure.Content = myItem.NameFigure;
                    var list = ShapeContainer.list;
                    foreach (var rect in list)
                    {
                        if (rect.shape.Name == myItem.NameFigure)
                        {
                            OpacitySlider.Value = rect.shape.Opacity;
                        }
                    }
                }
            }
            catch { }
        }
        public void RefreshListBox()
        {
            ListBoxAllElements.Items.Clear();
            var zxibit = ShapeContainer.list.Where(x => x.toFileName == paths[counterImage-1]).ToList();
            counter = 1;
            foreach (var x in zxibit)
            {
                ListBoxAllElements.Items.Add(new MyItem { Counter = counter, TypeFigure = "Rectangle" + counter, NameFigure = x.name, backgroundGrid = x.colorFill, shape = x.shape });
                counter++;
            }
        }
        /// <summary>
        /// Выбор цвета фигуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void X12_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (currentSelectedListBoxItem != null)
            {
                MyItem o = (MyItem)currentSelectedListBoxItem.DataContext;
                var m = ShapeContainer.list;
                foreach (var rect in m)
                {
                    if (rect.name == o.NameFigure)
                    {
                        Color color = X12.SelectedColor.Value;
                        
                        rect.shape.Fill = new SolidColorBrush(Color.FromArgb(40,(byte)color.R, (byte)color.G, (byte)color.B));
                        rect.shape.Stroke = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                        currentSelectedListBoxItem.Background = new SolidColorBrush(Color.FromArgb((byte)125, (byte)color.R, (byte)color.G, (byte)color.B));
                        PreviewColorBorder.Background = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                    }
                }
            }
        }

        private void DrawEllipseBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShapeCurrent = new Ellipse();
            CanvasDrawer.SelectedShape = ShapeCurrent; //Выбран режим Эллипса
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShapeCurrent = new Rectangle();
            CanvasDrawer.SelectedShape = ShapeCurrent; // Выбран режим Эллипса
        }
        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Thumbnails.SelectedItem != null)
            {
                var thumbnails = Thumbnails.SelectedItem.ToString();
                counterImage = Thumbnails.SelectedIndex + 1 + currentPageIndex * 10;
                CounterLabel.Content = counterImage.ToString();
                Cnv.Children.Clear();
                ImagePreview.Source = ImageController.correctImage(paths, counterImage-1);
                rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(rectangleShapesInPhoto);
                PrintImagesInPhoto(rectangleShapesInPhoto, listrect);
                RefreshListBox();
            }

        }
        public  void AddElementToListBox(MyItem myItem)
        {
            ListBoxAllElements.Items.Add(myItem);
        }
        
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage-1);
            var xa = rectangleShapesInPhoto.Select(x => x.shape).ToList();
            if (xa.Count > 0)
            {
                Cnv.Children.Remove(xa.Last());
                ShapeContainer.list.Remove(rectangleShapesInPhoto.Last());
                ListBoxAllElements.Items.RemoveAt(ListBoxAllElements.Items.Count - 1);
            }
        }
       
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (currentSelectedListBoxItem != null)
            {
                MyItem o = (MyItem)currentSelectedListBoxItem.DataContext;
                var itema = ShapeContainer.list;
                foreach (var rect in itema)
                {
                    if (rect.name == o.NameFigure)
                    {
                        rect.shape.Opacity = OpacitySlider.Value;
                        rect.opacity = OpacitySlider.Value;
                    }
                }
            }
        }
        private void NameFigureTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string theText = textBox.Text;
                NameFigure.Content = theText;
                MyItem myItem = (MyItem)currentSelectedListBoxItem.DataContext;
                myItem.NameFigure = theText;
                var lists = ShapeContainer.list;
                textBox.Width = textBox.Text.Length * 7;
                foreach (var i in lists)
                {
                    if (myItem.shape.Name == i.shape.Name && myItem.shape.Width == i.shape.Width && myItem.shape.Height == i.shape.Height)
                    {
                        i.name = myItem.NameFigure;
                        i.shape.Name = myItem.NameFigure;
                    }
                }


            }
        }
        private void SaveAllBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            String sPath_SubDirectory = SearchPage.catalog + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
            {
                Directory.CreateDirectory(sPath_SubDirectory);
            }
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(ShapeContainer.list, Formatting.Indented));
            MessageBox.Show("Данные сохранены.");
        }
        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String sPath_SubDirectory = SearchPage.catalog + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
            {
                Directory.CreateDirectory(sPath_SubDirectory);
            }
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(ShapeContainer.list));
            MessageBox.Show("Данные сохранены.");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            R = (byte)r.Next(1, 255);
            G = (byte)r.Next(1, 255);
            B = (byte)r.Next(1, 233);
            using var yolo = new Classes.Yolo.Models.Yolov8Net(Directory.GetCurrentDirectory() + @"\Assets\Weights\yolov8m.onnx");
            Image img = Image.FromFile(paths[counterImage - 1]);
            using var image = Image.FromFile(paths[counterImage-1]);
            var predictions = yolo.Predict(img);
            foreach (var pred in predictions)
            {
                GroupIDCounter++;
                var originalImageHeight = img.Height;
                var originalImageWidth  = img.Width;
                var x      = Math.Max(pred.Rectangle.X, 0);
                var y      = Math.Max(pred.Rectangle.Y, 0);
                var width  = Math.Min(originalImageWidth - x, pred.Rectangle.Width);
                var height = Math.Min(originalImageHeight - y, pred.Rectangle.Height);
                var name   = pred.Label.Name.Replace(" ", "_");
                ShapeCurrent = new Rectangle();
                Canvas.SetLeft(ShapeCurrent, x);
                Canvas.SetTop(ShapeCurrent, y);
                ShapeCurrent.Width   = width;
                ShapeCurrent.Height  = height;
                ShapeCurrent.StrokeThickness = 2;
                ShapeCurrent.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
                ShapeCurrent.Fill    = new SolidColorBrush(Color.FromArgb(40, R,B,G));
                ShapeCurrent.Name    = name;
                Canvas.SetLeft(ShapeCurrent, x);
                Canvas.SetTop(ShapeCurrent, y);
                TextBlock textblock = new();
                Canvas.SetLeft(textblock, x);
                Canvas.SetTop(textblock, y);
                textblock.Text = name;
                textblock.Name = name;
                AddChild(ShapeCurrent, GroupIDCounter);
                AddChild(textblock, GroupIDCounter);
                ShapeContainer.AddFigure(ShapeFigure.ShapeToFigure(ShapeCurrent, Math.Round(x, 4), Math.Round(y, 4), paths[counterImage-1], ShapeCurrent.Name, ShapeCurrent.Opacity, ShapeCurrent.Stroke));
                OpacitySlider.Value = ShapeCurrent.Opacity;
                RefreshListBox();
                
            }
        }
        public void AddChild(UIElement element, Int32 groupID)
        {
            try
            {
                UIElementExtensions.SetGroupID(element, groupID);
                Cnv.Children.Add(element);
            }
            catch { }
        }
        
    }
}
