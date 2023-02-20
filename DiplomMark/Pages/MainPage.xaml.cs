
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
using System.Windows.Shapes;
using Figure = DiplomMark.Classes.Figure;
using Image = System.Drawing.Image;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;
using ColorConverter = System.Windows.Media.ColorConverter;
using Point = System.Windows.Point;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static MainPage MainPageController;
        public static int CounterImage        = 1;
        public static RoutedCommand MyCommand = new();
        public static int GroupIDCounter      = 1;
        public static List<Figure> RectangleShapesInPhoto = new();
        public static List<string> Paths = new();
        private static List<SelectImages> _images = new();

        int counter = 0;
        int currentPageIndex = 0;
        int itemPerPage = 10;
        int totalPage = 0;
        int totalCount = 0;
        Random _rand = new();
        static byte _R;
        static byte _G;
        static byte _B;
        List<BitmapImage> _itemsList = new();
        static ListBoxItem _currentSelectedListBoxItem;
        public MainPage(List<SelectImages> images)
        {
            InitializeComponent();
            Cnv.Focus();
            Cnv.Focusable = true;
            _images = images;
            MyCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            #region Пагинирование
            AddPaths(_images);
            ImagePreview.Source = ImageController.FromFile(Paths[CounterImage]);
            MaxPhotosLabel.Content = Paths.Count;
            totalCount = Paths.Count;
            totalPage = totalCount / itemPerPage;
            if (totalCount % itemPerPage != 0)
            {
                totalPage += 1;
            }
            foreach (var fi in _images)
            {
                _itemsList.Add(new BitmapImage(new Uri(fi.URIToFile)));
            }
            for (int i = 0; i < _itemsList.ToList().Take(10).Count(); i++)
            {
                Thumbnails.Items.Add(_itemsList[i].UriSource.AbsolutePath);
            }
            Thumbnails.SelectedIndex = 0;
            #endregion
            OpenToSave();
            MainPageController = this;
        }
        /// <summary>
        /// Восстановление сохранения при открытии
        /// </summary>
        private void OpenToSave()
        {
            String sPath_SubDirectory = SearchPage.catalog + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false) return;
            if (File.Exists(sPath_SubDirectory + "\\saves.json"))
            {
                string text = File.ReadAllText(sPath_SubDirectory + "\\saves.json");
                var x = SerializeToJson.DeserealizingJSON(text);
                FiguresList.ListFigures.AddRange(x);
                RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage - 1);
                var listrect = TransformFigureTo.ListToPrintShapes(RectangleShapesInPhoto);
                PrintImagesInPhoto(RectangleShapesInPhoto, listrect);
                RefreshListBox();
            }
        }
        /// <summary>
        /// Добавление ссылок фотографии в List
        /// </summary>
        /// <param NameFigure="images"></param>
        private void AddPaths(List<SelectImages> images)
        {
            foreach (var fi in images)
            {
                Paths.Add(fi.URIToFile);
            }
        }
        /// <summary>
        /// Пагинация Листбокса
        /// </summary>
        /// <param NameFigure="flag">true = Пагинация на страницу вперед
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
                        Thumbnails.Items.Add(_itemsList[i].UriSource);
                    }
                }
                if (flag && currentPageIndex < totalPage - 1)
                {
                    Thumbnails.Items.Clear();

                    currentPageIndex++;
                    for (int i = currentPageIndex * 10; i < (currentPageIndex + 1) * 10; i++)
                    {
                        Thumbnails.Items.Add(_itemsList[i].UriSource);
                    }
                }
            }
            catch { }

        }
        /// <summary>
        /// Вывод фотографий на Canvas при переключении фото
        /// </summary>
        /// <param NameFigure="RectangleShapesInPhoto"></param>
        /// <param NameFigure="listrect"></param>
        public void PrintImagesInPhoto(List<Figure> rectangleShapesInPhoto, List<Shape> listrect)
        {
            foreach (var shapes in listrect)
            {
                foreach (var figures in rectangleShapesInPhoto)
                {
                    if (shapes.Width == figures.ShapeFigure.Width && shapes.Height == figures.ShapeFigure.Height && shapes.Name == figures.ShapeFigure.Name)
                    {
                        Canvas.SetLeft(shapes, figures.Coord_X);
                        Canvas.SetTop(shapes, figures.Coord_Y);
                        shapes.Opacity = figures.FigureOpacity;
                        shapes.Width = figures.ShapeFigure.Width;
                        shapes.Height = figures.ShapeFigure.Height;
                        shapes.Name = figures.ShapeFigure.Name;
                        shapes.Fill = figures.ColorFill;
                        shapes.Stroke = figures.StrokeFill;
                        shapes.StrokeThickness = figures.ShapeFigure.StrokeThickness;
                        figures.ShapeFigure = shapes;
                        Cnv.Children.Add(shapes);
                        TextBlock txtBox = new TextBlock() {  Width = 80, Text = shapes.Name, FontSize = 10, Name = shapes.Name };
                        Cnv.Children.Add(txtBox);
                        Canvas.SetLeft(txtBox, figures.Coord_X);
                        Canvas.SetTop(txtBox, figures.Coord_Y);
                    }
                }
            }
        }

        #region Перелистывание фоток
        private void LabelNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CounterImage < Convert.ToInt32(MaxPhotosLabel.Content))
            {
                CounterImage++;
                Cnv.Children.Clear();
                RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(RectangleShapesInPhoto);
                PrintImagesInPhoto(RectangleShapesInPhoto, listrect);
                Thumbnails.SelectedIndex = (CounterImage < 10) ? CounterImage - 1 : (CounterImage - 1) % 10;
                if ((CounterImage - 1) % 10 == 0)
                {
                    PaginationListbox(true);
                    Thumbnails.SelectedIndex = 0;
                }
                CounterLabel.Content = CounterImage;
                ImagePreview.Source = ImageController.CorrectingImage(Paths, CounterImage-1);
            }
        }
        private void BackLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CounterImage > 1)
            {
                CounterImage--;
                Cnv.Children.Clear();
                RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(RectangleShapesInPhoto);
                PrintImagesInPhoto(RectangleShapesInPhoto, listrect);
                Thumbnails.SelectedIndex = (CounterImage < 10) ? CounterImage - 1 : (CounterImage - 1) % 10;
                if ((CounterImage - 1) % 10 == 9){

                    PaginationListbox(false);
                    Thumbnails.SelectedIndex = 9;
                }
                CounterLabel.Content = CounterImage;
                ImagePreview.Source =  ImageController.CorrectingImage(Paths, CounterImage - 1);
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
        /// <param NameFigure="sender"></param>
        /// <param NameFigure="e"></param>
        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _currentSelectedListBoxItem = ListBoxAllElements.ItemContainerGenerator.ContainerFromIndex(ListBoxAllElements.SelectedIndex) as ListBoxItem;
                if (_currentSelectedListBoxItem != null)
                {
                    string colorValue = ((SolidColorBrush)_currentSelectedListBoxItem.Background).Color.ToString();
                    X12.SelectedColor = (Color)ColorConverter.ConvertFromString(colorValue);
                    MyItem myItem = (MyItem)_currentSelectedListBoxItem.DataContext;
                    NameFigure.Content = myItem.NameFigure;
                    var list = FiguresList.ListFigures;
                    foreach (var rect in list)
                    {
                        if (rect.ShapeFigure.Name == myItem.NameFigure)
                            OpacitySlider.Value = rect.ShapeFigure.Opacity;
                    }
                }
            }
            catch { }
        }
        public void RefreshListBox()
        {
            ListBoxAllElements.Items.Clear();
            var zxibit = FiguresList.ListFigures.Where(x => x.ToFileName == Paths[CounterImage-1]).ToList();
            counter = 1;
            foreach (var x in zxibit)
            {
                ListBoxAllElements.Items.Add(new MyItem { Counter = counter, TypeFigure = "Rectangle" + counter, NameFigure = x.NameFigure, BackgroundGrid = x.ColorFill, FigureShape = x.ShapeFigure });
                counter++;
            }
        }
        /// <summary>
        /// Выбор цвета фигуры
        /// </summary>
        /// <param NameFigure="sender"></param>
        /// <param NameFigure="e"></param>
        private void X12_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (_currentSelectedListBoxItem != null)
            {
                MyItem o = (MyItem)_currentSelectedListBoxItem.DataContext;
                var m = FiguresList.ListFigures;
                foreach (var rect in m)
                {
                    if (rect.NameFigure == o.NameFigure)
                    {
                        Color color = X12.SelectedColor.Value;
                        rect.ShapeFigure.Fill = new SolidColorBrush(Color.FromArgb(40,(byte)color.R, (byte)color.G, (byte)color.B));
                        rect.ShapeFigure.Stroke = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                        _currentSelectedListBoxItem.Background = new SolidColorBrush(Color.FromArgb((byte)125, (byte)color.R, (byte)color.G, (byte)color.B));
                        PreviewColorBorder.Background = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                    }
                }
            }
        }
        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Thumbnails.SelectedItem != null)
            { 
                CounterImage = Thumbnails.SelectedIndex + 1 + currentPageIndex * 10;
                CounterLabel.Content = CounterImage.ToString();
                Cnv.Children.Clear();
                ImagePreview.Source = ImageController.CorrectingImage(Paths, CounterImage-1);
                RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(RectangleShapesInPhoto);
                PrintImagesInPhoto(RectangleShapesInPhoto, listrect);
                RefreshListBox();
            }

        }
        public  void AddElementToListBox(MyItem myItem)
        {
            ListBoxAllElements.Items.Add(myItem);
        }
        
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage-1);
            var ShapeFigures = RectangleShapesInPhoto.Select(x => x.ShapeFigure).ToList();
            if (ShapeFigures.Count > 0)
            {
                Cnv.Children.Remove(ShapeFigures.Last());
                FiguresList.ListFigures.Remove(RectangleShapesInPhoto.Last());
                ListBoxAllElements.Items.RemoveAt(ListBoxAllElements.Items.Count - 1);
            }
        }
       
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_currentSelectedListBoxItem != null)
            {
                MyItem o = (MyItem)_currentSelectedListBoxItem.DataContext;
                var itema = FiguresList.ListFigures;
                foreach (var rect in itema)
                {
                    if (rect.NameFigure == o.NameFigure)
                    {
                        rect.ShapeFigure.Opacity = OpacitySlider.Value;
                        rect.FigureOpacity = OpacitySlider.Value;
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
                MyItem myItem = (MyItem)_currentSelectedListBoxItem.DataContext;
                myItem.NameFigure = theText;
                var lists = FiguresList.ListFigures;
                textBox.Width = textBox.Text.Length * 7;
                foreach (var i in lists)
                {
                    if (myItem.FigureShape.Name == i.ShapeFigure.Name && myItem.FigureShape.Width == i.ShapeFigure.Width && myItem.FigureShape.Height == i.ShapeFigure.Height)
                    {
                        i.NameFigure = myItem.NameFigure;
                        i.ShapeFigure.Name = myItem.NameFigure;
                    }
                }
            }
        }
        private void SaveAllBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            String sPath_SubDirectory = SearchPage.catalog + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
                Directory.CreateDirectory(sPath_SubDirectory);
            
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(FiguresList.ListFigures, Formatting.Indented));
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
                Directory.CreateDirectory(sPath_SubDirectory);
            
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(FiguresList.ListFigures));
            MessageBox.Show("Данные сохранены.");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _R = (byte)_rand.Next(1, 255);
            _G = (byte)_rand.Next(1, 255);
            _B = (byte)_rand.Next(1, 233);
            using var yolo = new Classes.Yolo.Models.Yolov8Net(Directory.GetCurrentDirectory() + @"\Assets\Weights\yolov8m.onnx");
            Image img = Image.FromFile(Paths[CounterImage - 1]);
            using var image = Image.FromFile(Paths[CounterImage-1]);
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
                var ShapeCurrent = new Rectangle();
                Canvas.SetLeft(ShapeCurrent, x);
                Canvas.SetTop(ShapeCurrent, y);
                ShapeCurrent.Width   = width;
                ShapeCurrent.Height  = height;
                ShapeCurrent.StrokeThickness = 2;
                ShapeCurrent.Stroke  = new SolidColorBrush(Color.FromRgb(_R, _G, _B));
                ShapeCurrent.Fill    = new SolidColorBrush(Color.FromArgb(40, _R,_B,_G));
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
                FiguresList.AddFigure(ShapeFigure.ShapeToFigure(ShapeCurrent, Math.Round(x, 4), Math.Round(y, 4), Paths[CounterImage-1], ShapeCurrent.Name, ShapeCurrent.Opacity, ShapeCurrent.Stroke));
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
