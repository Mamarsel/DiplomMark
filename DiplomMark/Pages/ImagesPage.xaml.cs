using Aspose.Html.IO;
using DiplomMark.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для ImagesPage.xaml
    /// </summary>
    public partial class ImagesPage : Page
    {

        static List<SelectImages> images = new();
        static string[] filesImages;
        public ImagesPage()
        {
            InitializeComponent();

            string[] FilesJpg = Directory.GetFiles(SearchPage.catalog, "*.jpg");
            string[] FilesPng = Directory.GetFiles(SearchPage.catalog, "*.png");
            filesImages = FilesJpg.Concat(FilesPng).ToArray();
            foreach (var fi in filesImages)
                images.Add(new SelectImages { imageSource = BitmapFromUri(new Uri(fi)), uritoFile = fi, isCheck = true });
            GetResolutionAllImages(images);

            images = SetResolutionAllImages(images);
            imageResolution.Insert(0, new string("Все типы"));
            imageResolution.ToList().ForEach(x => ResolutionsCB.Items.Add(x));


            ResolutionsCB.SelectedIndex = 0;
            UpdateThumbnails(images);
           
        }
        List<String> imageResolution = new();
        private void GetResolutionAllImages(List<SelectImages> image)
        {
            foreach (var file in image)
            {
                using (var fileStream = new FileStream(file.uritoFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var img = System.Drawing.Image.FromStream(fileStream, false, false))
                    {
                        //file.imageWidth = img.Width;
                        //file.imageHeight = img.Height;
                        if (!imageResolution.Contains($"{img.Width}x{img.Height}"))
                        {
                            imageResolution.Add($"{img.Width}x{img.Height}");
                        }
                    }
                }
            }
        }
        private List<SelectImages> SetResolutionAllImages(List<SelectImages> image)
        {
            foreach (var file in image)
            {
                using (var fileStream = new FileStream(file.uritoFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var img = System.Drawing.Image.FromStream(fileStream, false, false))
                    {
                        file.imageWidth = img.Width;
                        file.imageHeight = img.Height;
                    }
                }
            }
            return image;
        }
        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        private void RefreshThumb(List<SelectImages> images)
        {
            Thumbnails.ItemsSource = null;
            foreach (var fi in images)
            {
                File.Delete(fi.uritoFile);
                
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deleteimage = images.Where(x => x.isCheck == false).ToList();
                images.RemoveAll(x => x.isCheck == false);
                RefreshThumb(deleteimage);
                Thumbnails.ItemsSource = images;
                MainWindow.main.MainFrame.Navigate(new MainPage(images));
            }
            catch { }
        }

        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_item = Thumbnails.SelectedItem as SelectImages;
            if (selected_item == null)
                return;
            var selectedimage = images.FirstOrDefault(x => x.uritoFile == selected_item.uritoFile);
            var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(Thumbnails.SelectedIndex) as ListBoxItem;
            ContentPresenter myContentPresenter = VisualFindChild.FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);

            if (myContentPresenter == null) { return; }
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            Image target = (Image)myDataTemplate.FindName("CBSelected", myContentPresenter);
            if (!selectedimage.isCheck)
            {
                target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/correct.png"));
                images.FirstOrDefault(x => x.uritoFile == selected_item.uritoFile).isCheck = true;
                Thumbnails.SelectedItem = null;
            }
            else if (selectedimage.isCheck)
            {
                target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/remove.png"));
                images.FirstOrDefault(x => x.uritoFile == selected_item.uritoFile).isCheck = false;
                Thumbnails.SelectedItem = null;
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            images.ToList().ForEach(x => x.isCheck = CbCheck.IsChecked.Value);
            CheckAllItems();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            images.ToList().ForEach(x => x.isCheck = CbCheck.IsChecked.Value);
            CheckAllItems();
        }
        private void UpdateThumbnails(List<SelectImages> images)
        {
            Thumbnails.ItemsSource = null;
            Thumbnails.Items.Clear();
            images = SetResolutionAllImages(images);
            if (ResolutionsCB != null)
                if (ResolutionsCB.SelectedIndex > 0)
                {
                    List<SelectImages> temp = new();
                    foreach (var item in images)
                    {
                        if (($"{item.imageWidth}x{item.imageHeight}" == ResolutionsCB.SelectedItem.ToString()))
                        {
                            temp.Add(item);
                        }
                    }
                    images = temp;

                }
            
           
            for (int i = 0; i < images.Count; i++)
            {
                Thumbnails.ItemsSource = images;
            }
            CheckAllItems();

        }
        private void CheckAllItems()
        {
            for (int i = 0; i < Thumbnails.Items.Count; i++)
            {
                var data = Thumbnails.Items[i] as SelectImages;
                if (data == null)
                    return;
                var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                ContentPresenter myContentPresenter = VisualFindChild.FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);
                if (myContentPresenter == null) { continue; }
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                Image target = (Image)myDataTemplate.FindName("CBSelected", myContentPresenter);
                if (data.isCheck)
                {
                    target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/correct.png"));
                }
                else
                {
                    target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/remove.png"));
                }

            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateThumbnails(images);
            CheckAllItems();
        }
    }
}
