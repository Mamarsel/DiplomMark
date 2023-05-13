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
using Figure = DiplomMark.Classes.Figures.Figure;
using Image = System.Drawing.Image;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;
using ColorConverter = System.Windows.Media.ColorConverter;
using Point = System.Windows.Point;
using DiplomMark.Classes.DatabaseClasses;
using System.Linq.Expressions;
using DiplomMark.Classes.HelpClasses;
using DiplomMark.Classes.Figures;

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
        private static string[] _images;

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
        public MainPage(string[] images)
        {
            InitializeComponent();
            Cnv.Focus();
            Cnv.Focusable = true;
            _images = images;
            MyCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            #region Пагинирование
            AddPaths(_images);
            ImagePreview.Source = ImageController.FromFile(Paths[CounterImage-1]);
            //MaxPhotosLabel.Content = Paths.Count;
            totalCount = Paths.Count;
            totalPage = totalCount / itemPerPage;
            if (totalCount % itemPerPage != 0)
            {
                totalPage += 1;
            }
            foreach (var fi in _images)
            {
                _itemsList.Add(new BitmapImage(new Uri(fi)));
            }
            for (int i = 0; i < _itemsList.ToList().Take(10).Count(); i++)
            {
                Thumbnails.Items.Add(_itemsList[i].UriSource.AbsolutePath);
            }
            Thumbnails.SelectedIndex = 0;
            #endregion
            OpenToSave();
            if(FiguresList.ListFigures.Count > 0)
                AddTagsSave(FiguresList.ListFigures);
            MainPageController = this;
            ListBoxAllElements.Focus();
        }
        /// <summary>
        /// Восстановление сохранения при открытии
        /// </summary>
        private void OpenToSave()
        {
            String sPath_SubDirectory = SearchPage.SelectedCatalogs + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false || Cnv.Children.Count > 0) return;
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
        private void AddTagsSave(List<Figure> figures)
        {
            var tagClasses = figures.Select(x=> new TagClass { TagColor = (SolidColorBrush)x.ColorFill, TagName = x.NameFigure}).DistinctBy(x=>x.TagName).ToList();
            GlobalVars.Tags.AddRange(tagClasses);
            ListBoxAllElements.ItemsSource = GlobalVars.Tags.DistinctBy(x=>x.TagName);
        }
        /// <summary>
        /// Добавление ссылок фотографии в List
        /// </summary>
        /// <param NameFigure="images"></param>
        private void AddPaths(string[] images)
        {
            foreach (var fi in images)
            {
                Paths.Add(fi);
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
        /// <param NameFigure="RectangleShapesInPhoto">Прямоугольники на фото</param>
        /// <param NameFigure="listrect">Лист со всеми фигурами</param>
        public void PrintImagesInPhoto(List<Figure> rectangleShapesInPhoto, List<Shape> listrect)
        {
            Cnv.Children.Clear();
            foreach (var shapes in listrect)
            {
                foreach (var figures in rectangleShapesInPhoto)
                {
                    if (shapes.Width == figures.ShapeFigure.Width && shapes.Height == figures.ShapeFigure.Height && shapes.Name == figures.ShapeFigure.Name)
                    {
                        
                        shapes.Opacity = figures.FigureOpacity;
                        shapes.Width = figures.ShapeFigure.Width;
                        shapes.Height = figures.ShapeFigure.Height;
                        shapes.Name = figures.ShapeFigure.Name;
                        shapes.Fill = figures.ColorFill;
                        shapes.Stroke = figures.StrokeFill;
                        shapes.StrokeThickness = figures.ShapeFigure.StrokeThickness;
                        figures.ShapeFigure = shapes;
                        Canvas.SetLeft(shapes, figures.Coord_X);
                        Canvas.SetTop(shapes, figures.Coord_Y);
                        Cnv.Children.Remove(shapes);
                        Cnv.Children.Add(shapes);
                    }
                }
            }
        }

        #region Перелистывание фоток
        private void LabelNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CounterImage < Convert.ToInt32(Paths.Count))
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
                //CounterLabel.Content = CounterImage;
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
                //CounterLabel.Content = CounterImage;
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
        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _currentSelectedListBoxItem = ListBoxAllElements.ItemContainerGenerator.ContainerFromIndex(ListBoxAllElements.SelectedIndex) as ListBoxItem;
                if (_currentSelectedListBoxItem != null)
                {
                    //string colorValue = ((SolidColorBrush)_currentSelectedListBoxItem.Background).Color.ToString();
                    //X12.SelectedColor = (Color)ColorConverter.ConvertFromString(colorValue);
                    TagClass myItem = (TagClass)_currentSelectedListBoxItem.DataContext;
                    GlobalVars.SelectedTag = myItem;
                    var list = FiguresList.ListFigures;             }
            }
            catch { }
        }
        public void RefreshListBox()
        {
            //ListBoxAllElements.Items.Clear();
            //var zxibit = FiguresList.ListFigures.Where(x => x.ToFileName == Paths[CounterImage-1]).ToList();
            //counter = 1;
            //foreach (var x in zxibit)
            //{
            //    ListBoxAllElements.Items.Add(new TagClass { Counter = counter, TypeFigure = "Rectangle" + counter, NameFigure = x.NameFigure, BackgroundGrid = x.ColorFill, FigureShape = x.ShapeFigure });
            //    counter++;
            //}
        }
        /// <summary>
        /// Выбор цвета фигуры
        /// </summary>
        /// <param NameFigure="sender"></param>
        /// <param NameFigure="e"></param>
        private void X12_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            //if (_currentSelectedListBoxItem != null)
            //{
            //    MyItem selecteditem = (MyItem)_currentSelectedListBoxItem.DataContext;
            //    var shapes = FiguresList.ListFigures;
            //    foreach (var rect in shapes)
            //    {
            //        if (rect.NameFigure == selecteditem.NameFigure)
            //        {
            //            Color color = X12.SelectedColor.Value;
            //            rect.ShapeFigure.Fill = new SolidColorBrush(Color.FromArgb(40,(byte)color.R, (byte)color.G, (byte)color.B));
            //            rect.ShapeFigure.Stroke = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
            //            _currentSelectedListBoxItem.Background = new SolidColorBrush(Color.FromArgb((byte)125, (byte)color.R, (byte)color.G, (byte)color.B));
            //            PreviewColorBorder.Background = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
            //        }
            //    }
            //}
        }
        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Thumbnails.SelectedItem != null)
            { 
                CounterImage = Thumbnails.SelectedIndex + 1 + currentPageIndex * 10;
                //CounterLabel.Content = CounterImage.ToString();
                Cnv.Children.Clear();
                ImagePreview.Source = ImageController.CorrectingImage(Paths, CounterImage-1);
                RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage-1);
                var listrect = TransformFigureTo.ListToPrintShapes(RectangleShapesInPhoto);
                PrintImagesInPhoto(RectangleShapesInPhoto, listrect);
                RefreshListBox();
            }

        }
      
        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void NameFigureTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string theText = textBox.Text;
             
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
            String sPath_SubDirectory = SearchPage.SelectedCatalogs + "\\" + "Saves";
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
            String sPath_SubDirectory = SearchPage.SelectedCatalogs + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
                Directory.CreateDirectory(sPath_SubDirectory);
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(FiguresList.ListFigures));
            MessageBox.Show("Данные сохранены.");
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExitCommand_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage - 1);
            var ShapeFigures = RectangleShapesInPhoto.Select(x => x.ShapeFigure).ToList();
            if (ShapeFigures.Count > 0)
            {
                Cnv.Children.Remove(ShapeFigures.Last());
                FiguresList.ListFigures.Remove(RectangleShapesInPhoto.Last());
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            String sPath_SubDirectory = SearchPage.SelectedCatalogs + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
                Directory.CreateDirectory(sPath_SubDirectory);
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(FiguresList.ListFigures));
            MessageBox.Show("Данные сохранены.");
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AIBTN_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            using(var db = new ApplicationContext())
            {
                var x = db.SettingsUser.FirstOrDefault();
                if(!x.GuideViewAI)
                {
                    MessageBox.Show("Нейросеть опирается на список ваших тегов.\nЕсли выделяемый объект есть в вашей коллекции тэгов, то он его выделяет");
                    x.GuideViewAI = true;
                }
            }
            using var yolo = new Classes.Yolo.Models.Yolov8Net(Directory.GetCurrentDirectory() + @"\Assets\Weights\yolov8m.onnx");
            Image img = Image.FromFile(Paths[CounterImage - 1]);
            using var image = Image.FromFile(Paths[CounterImage - 1]);
            var predictions = yolo.Predict(img);
            foreach (var pred in predictions)
            {
                GroupIDCounter++;
                var originalImageHeight = img.Height;
                var originalImageWidth  = img.Width;
                var x                   = Math.Max(pred.Rectangle.X, 0);
                var y                   = Math.Max(pred.Rectangle.Y, 0);
                var width               = Math.Min(originalImageWidth - x, pred.Rectangle.Width);
                var height              = Math.Min(originalImageHeight - y, pred.Rectangle.Height);
                var name                = pred.Label.Name.Replace(" ", "_");
                var tag                 = GlobalVars.Tags.FirstOrDefault(x => x.TagName == name);
                var ShapeCurrent        = new Rectangle();
                Canvas.SetLeft(ShapeCurrent, x);
                Canvas.SetTop(ShapeCurrent, y);
                if(tag!= null)
                {
                    ShapeCurrent = new Rectangle { Width = width, Height = height, StrokeThickness = 2, Stroke = tag.TagColor, Fill = tag.TagColor, Name = name };

                    Canvas.SetLeft(ShapeCurrent, x);
                    Canvas.SetTop(ShapeCurrent, y);
                    Cnv.Children.Add(ShapeCurrent);
                    FiguresList.AddFigure(ShapeFigure.ShapeToFigure(ShapeCurrent, Math.Round(x, 4), Math.Round(y, 4), Paths[CounterImage - 1], ShapeCurrent.Name, ShapeCurrent.Opacity, ShapeCurrent.Stroke));
                }
            }
        }

        private void NextPhotoImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CounterImage < Convert.ToInt32(Paths.Count))
            {
                CounterImage++;
                Cnv.Children.Clear();
                RectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(FiguresList.ListFigures, Paths, CounterImage - 1);
                var listrect = TransformFigureTo.ListToPrintShapes(RectangleShapesInPhoto);
                PrintImagesInPhoto(RectangleShapesInPhoto, listrect);
                Thumbnails.SelectedIndex = (CounterImage < 10) ? CounterImage - 1 : (CounterImage - 1) % 10;
                if ((CounterImage - 1) % 10 == 0)
                {
                    PaginationListbox(true);
                    Thumbnails.SelectedIndex = 0;
                }
                //CounterLabel.Content = CounterImage;
                ImagePreview.Source = ImageController.CorrectingImage(Paths, CounterImage - 1);
            }
        }
        private void NewPhotosImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.MainFrame.Navigate(new SearchPage(Paths.ToArray()));
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TagAddWindow m = new TagAddWindow();
            if (!GlobalVars.IsWindowTagOpen)
            {
                m.Show();
                GlobalVars.IsWindowTagOpen = true;
            }

        }

        private void NameFigureTB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
           
        }

        private void NameFigureTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                    _currentSelectedListBoxItem = ListBoxAllElements.ItemContainerGenerator.ContainerFromIndex(ListBoxAllElements.SelectedIndex) as ListBoxItem;

                if (_currentSelectedListBoxItem != null)
                {
                    TagClass myItem = _currentSelectedListBoxItem.DataContext as TagClass;

                    if (myItem != null)
                    {
                        FiguresList.ListFigures.RemoveAll(x => x.NameFigure == myItem.TagName);
                        Cnv.Children.Clear();
                        FiguresList.ListFigures.ForEach(x => Cnv.Children.Add(x.ShapeFigure));
                        
                    }
                    GlobalVars.Tags.Remove(myItem);
                    ListBoxAllElements.SelectedItem = null;
                    
                    ListBoxAllElements.ItemsSource = null;
                    ListBoxAllElements.ItemsSource = GlobalVars.Tags;
                    ListBoxAllElements.Items.Refresh();
                    GlobalVars.SelectedTag = null;
                }
            }
            catch { }
        }
    }
}
