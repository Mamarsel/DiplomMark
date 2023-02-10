
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

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static int counterImage = 1;
        public static RoutedCommand MyCommand = new();

        private Point startPoint;
        private Rectangle rect;
        private Ellipse ellipse;

        int counter = 0;
        int currentPageIndex = 0;
        int itemPerPage = 10;
        int totalPage = 0;
        int totalCount = 0;

        List<BitmapImage> itemsList = new();
        List<string> paths = new List<string>();
        public static List<Figure> rectangleShapesInPhoto = new();
        static ListBoxItem currentSelectedListBoxItem;
        Random r = new();
        ApplicationContext db = new();
        static byte R;
        static byte G;
        static byte B;
        static List<SelectImages> _images = new();
        public MainPage()
        {
            InitializeComponent();
            CanvasDrawer.SelectedShape = rect;

            DirectoryInfo di = new(@"C:\Users\JutsPC\Desktop\Diplom\парсер2\фото");
            MyCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            foreach (var fi in di.GetFiles())
            {
                paths.Add(fi.FullName);
            }
           
            ImagePreview.Source = ImageController.correctImage(paths, counterImage-1);
            MaxPhotosLabel.Content = paths.Count;
            totalCount = paths.Count;
            totalPage = totalCount / itemPerPage;
            if (totalCount % itemPerPage != 0)
            {
                totalPage += 1;
            }
            foreach (var fi in di.GetFiles())
            {
                itemsList.Add(new BitmapImage(new Uri(fi.FullName)));
            }
            for (int i = 0; i < 10; i++)
            {
                Thumbnails.Items.Add(itemsList[i].UriSource);
            }
            Thumbnails.SelectedIndex = 0;
        }

        public MainPage(List<SelectImages> images)
        {

            InitializeComponent();
            CanvasDrawer.SelectedShape = rect;
            _images = images;
            Cnv.Height = ImagePreview.Height;
            Cnv.Width = ImagePreview.Width;
            MyCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            #region Пагинирование
            foreach (var fi in _images)
            {
                paths.Add(fi.uritoFile);
            }
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
            for (int i = 0; i < itemsList.Count%11; i++)
            {
                Thumbnails.Items.Add(itemsList[i].UriSource.AbsolutePath);
            }
            Thumbnails.SelectedIndex = 0;
            #endregion

            String sPath_SubDirectory = SearchPage.catalog + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
            {
                return;
            }
            if(!File.Exists(sPath_SubDirectory + "\\saves.json"))
            {
                string text = File.ReadAllText(sPath_SubDirectory + "\\saves.json");
                var x = SerializeToJson.DeserealizingJSON(text);
                ShapeContainer.list.AddRange(x);
                rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage);
                var listrect = TransformFigureTo.ListToPrintShapes(rectangleShapesInPhoto);
                PrintImagesInPhoto(rectangleShapesInPhoto, listrect);
                RefreshListBox();
            }

          

        }
       
        private void PaginationListbox(bool flag)
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
                        shapes.Fill = figures.shape.Fill;
                        shapes.Stroke = figures.shape.Stroke;
                        shapes.StrokeThickness = figures.shape.StrokeThickness;
                        figures.shape = shapes;
                        Cnv.Children.Add(shapes);
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
                if ((counterImage - 1) % 10 == 9)
                {

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
            catch
            {

            }
        }
        private void RefreshListBox()
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
                        rect.shape.Fill = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                        currentSelectedListBoxItem.Background = new SolidColorBrush(Color.FromArgb((byte)125, (byte)color.R, (byte)color.G, (byte)color.B));
                        PreviewColorBorder.Background = new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                    }
                }
            }

        }

        private void DrawEllipseBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CanvasDrawer.SelectedShape = ellipse;
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CanvasDrawer.SelectedShape = rect;
        }
        /// <summary>
        /// Добавление фигуры в статический список ShapeContainer.list для дальнейшей работы с ним
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridImage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var a = (MyItem)ListBoxAllElements.Items[ListBoxAllElements.Items.Count - 1];
            if (CanvasDrawer.SelectedShape == rect)
            {
                ShapeContainer.AddFigure(RectangleShape.RectangleToFigure(rect, Math.Round(CanvasDrawer.coord_x, 4), Math.Round(CanvasDrawer.coord_y, 4), paths[counterImage-1], a.NameFigure, rect.Opacity));
                OpacitySlider.Value = rect.Opacity;
                FiguresOnImage n = new FiguresOnImage() { Coord_X = CanvasDrawer.coord_x, Coord_Y = CanvasDrawer.coord_y, Height = rect.Height, Width = rect.Width, URLToImage = paths[counterImage-1], NameFigure = a.NameFigure };
                db.figuresOnImages.Add(n);
                
                foreach(var b in db.figuresOnImages)
                {
                    MessageBox.Show("Название: "+b.NameFigure + "\nКоордината X:" + b.Coord_X.ToString() + "\n Координата Y:" + b.Coord_Y + "\n Высота:" + b.Height +
                        "\nШирина:" + b.Width);
                }
               
            }
            else if (CanvasDrawer.SelectedShape == ellipse)
            {
                ShapeContainer.AddFigure(EllipseShape.EllipseToFigure(ellipse, Math.Round(CanvasDrawer.coord_x, 4), Math.Round(CanvasDrawer.coord_y, 4), paths[counterImage-1], a.TypeFigure, rect.Opacity));
                OpacitySlider.Value = ellipse.Opacity;
                FiguresOnImage n = new FiguresOnImage() { Coord_X = CanvasDrawer.coord_x, Coord_Y = CanvasDrawer.coord_y, Height = rect.Height, Width = rect.Width, URLToImage = paths[counterImage - 1], NameFigure = a.NameFigure };
                db.figuresOnImages.Add(n);
            }
            X12.SelectedColor = Color.FromRgb(R, G, B);
            PreviewColorBorder.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            db.SaveChanges();

        }
        #region Рисование
        /// <summary>
        /// Отрисовка фигур по движению мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveOnFigureAndCanvas(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || rect == null || ellipse == null || Keyboard.IsKeyDown(Key.LeftCtrl))
                return;
            var pos = e.GetPosition(Cnv);
            CanvasDrawer c = new CanvasDrawer(CanvasDrawer.SelectedShape);
            c.DrawRectangle(startPoint, pos);
        }

        private void MouseDownOnFigureAndCanvas(object sender, MouseButtonEventArgs e)//Метод для создания фигуры по нажатию мыши, который нужно оптимизировать
        {

            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                R = (byte)r.Next(1, 255);
                G = (byte)r.Next(1, 255);
                B = (byte)r.Next(1, 233);
                startPoint = e.GetPosition(Cnv);
                if (CanvasDrawer.SelectedShape == rect)
                {
                    ellipse = new Ellipse();
                    rect = new Rectangle();
                    CanvasDrawer.SelectedShape = rect;
                    rect.Fill = new SolidColorBrush(Color.FromArgb(40,R, G, B));
                    rect.Stroke = new SolidColorBrush(Color.FromRgb(R,G,B));
                    rect.StrokeThickness = 2;
                    
                    counter++;
                    rect.Name = "Rectangle" + counter;
                    rect.MinHeight = 20;
                    rect.MinWidth = 20;
                    Canvas.SetLeft(rect, startPoint.X);
                    Canvas.SetTop(rect, startPoint.Y);
                    Cnv.Children.Add(rect);
                    ListBoxAllElements.Items.Add(new MyItem { Counter = counter, TypeFigure = "RECTANGLE SHAPE", backgroundGrid = new SolidColorBrush(Color.FromRgb(R, G, B)), shape = rect, NameFigure = rect.Name });
                    ListBoxAllElements.SelectedIndex = ListBoxAllElements.Items.Count - 1;
                    TextBlock txtBox = new TextBlock() { Width = 80, Text = rect.Name, FontSize = 10, Name = rect.Name };
                    Cnv.Children.Add(txtBox);
                    Canvas.SetLeft(txtBox, startPoint.X);
                    Canvas.SetTop(txtBox, startPoint.Y);
                    txtBox.Focus();

                }
                else if (CanvasDrawer.SelectedShape == ellipse)
                {
                    rect = new Rectangle();
                    ellipse = new Ellipse();
                    CanvasDrawer.SelectedShape = ellipse;
                    ellipse.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
                    ellipse.Stroke = new SolidColorBrush(Colors.Gray);
                    counter++;
                    ellipse.Name = "Ellipse" + counter;
                    Canvas.SetLeft(ellipse, startPoint.X);
                    Canvas.SetTop(ellipse, startPoint.Y);
                    Cnv.Children.Add(ellipse);
                    ListBoxAllElements.Items.Add(new MyItem { Counter = counter, TypeFigure = "RECTANGLE SHAPE", backgroundGrid = new SolidColorBrush(Color.FromRgb(R, G, B)), shape = ellipse, NameFigure = ellipse.Name });
                    ListBoxAllElements.SelectedIndex = ListBoxAllElements.Items.Count - 1;
                }
            }
        }
        #endregion



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
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            rectangleShapesInPhoto = TransformFigureTo.ListRectangleInPhoto(ShapeContainer.list, paths, counterImage);
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
        private void Cnv_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var canvas = sender as Canvas;
                if (canvas == null)
                    return;

                HitTestResult hitTestResult = VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas));
                var element = (Shape)hitTestResult.VisualHit;
                element.Fill = new SolidColorBrush(Colors.Black);
            }
            
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String sPath_SubDirectory = SearchPage.catalog + "\\" + "Saves";
            if (Directory.Exists(sPath_SubDirectory) == false)
            {
                Directory.CreateDirectory(sPath_SubDirectory);
            }
            File.WriteAllText(sPath_SubDirectory + "\\saves.json", JsonConvert.SerializeObject(ShapeContainer.list[1]));
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
            using var yolo = new Classes.Yolo.Models.Yolov8Net(Directory.GetParent( Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName + "\\Assets\\Weights\\yolov8m.onnx");
            Image img = Image.FromFile(paths[counterImage - 1]);
            using var image = Image.FromFile(paths[counterImage-1]);
            var predictions = yolo.Predict(img);
            foreach (var pred in predictions)
            {
                var originalImageHeight = img.Height;
                var originalImageWidth  = img.Width;

                var x      = Math.Max(pred.Rectangle.X, 0);
                var y      = Math.Max(pred.Rectangle.Y, 0);
                var width  = Math.Min(originalImageWidth - x, pred.Rectangle.Width);
                var height = Math.Min(originalImageHeight - y, pred.Rectangle.Height);

                var name   = pred.Label.Name.Replace(" ", "_");
                Rectangle rect = new Rectangle();
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                rect.Width   = width;
                rect.Height  = height;
               
                rect.StrokeThickness = 2;
                rect.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
                rect.Fill    = new SolidColorBrush(Color.FromArgb(40, R,B,G));
                rect.Name    = name;
                Cnv.Children.Add(rect);

                TextBlock txtBox = new TextBlock() { Width = 80, Text = rect.Name, FontSize = 10, Name = rect.Name };
                Cnv.Children.Add(txtBox);
                Canvas.SetLeft(txtBox, x);
                Canvas.SetTop(txtBox, y);

                ShapeContainer.AddFigure(RectangleShape.RectangleToFigure(rect, Math.Round(x, 4), Math.Round(y, 4), paths[counterImage-1], rect.Name, rect.Opacity));
                OpacitySlider.Value = rect.Opacity;
                RefreshListBox();

            }
            
        }
    }
}
