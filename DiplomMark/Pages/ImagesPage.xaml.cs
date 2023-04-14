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
        public ImagesPage(string[] paths)
        {
            InitializeComponent();
            //string[] FilesJpg = Directory.GetFiles(SearchPage.SelectedCatalogs, "*.jpg");
            //string[] FilesPng = Directory.GetFiles(SearchPage.SelectedCatalogs, "*.png");
            //filesImages = FilesJpg.Concat(FilesPng).ToArray();
            foreach (var fi in paths)
                images.Add(new SelectImages { SourceImage = BitmapFromUri(new Uri(fi)), URIToFile = fi, IsCheck = true }); 
            UpdateThumbnails(images);
           
        }
        List<String> imageResolution = new();
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
                File.Delete(fi.URIToFile);
                
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
               
                var deleteimage = images.Where(x => x.IsCheck == false).ToList();
                images.RemoveAll(x => x.IsCheck == false);
                RefreshThumb(deleteimage);
                string[] strPaths = new string[images.Count-1];
                Thumbnails.ItemsSource = images;
                for(int i = 0; i < images.Count-1; i++)
                {
                    strPaths[i] = images[i].URIToFile;
                }

                MainWindow.main.MainFrame.Navigate(new MainPage(strPaths));
           
        }

        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_item = Thumbnails.SelectedItem as SelectImages;
            if (selected_item == null)
                return;
            var selectedimage = images.FirstOrDefault(x => x.URIToFile == selected_item.URIToFile);
            var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(Thumbnails.SelectedIndex) as ListBoxItem;
            ContentPresenter myContentPresenter = VisualFindChild.FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);

            if (myContentPresenter == null) { return; }
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            Image target = (Image)myDataTemplate.FindName("CBSelected", myContentPresenter);
            if (!selectedimage.IsCheck)
            {
                target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/correct.png"));
                images.FirstOrDefault(x => x.URIToFile == selected_item.URIToFile).IsCheck = true;
                Thumbnails.SelectedItem = null;
            }
            else if (selectedimage.IsCheck)
            {
                target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/remove.png"));
                images.FirstOrDefault(x => x.URIToFile == selected_item.URIToFile).IsCheck = false;
                Thumbnails.SelectedItem = null;
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            images.ToList().ForEach(x => x.IsCheck = CbCheck.IsChecked.Value);
            CheckAllItems();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            images.ToList().ForEach(x => x.IsCheck = CbCheck.IsChecked.Value);
            CheckAllItems();
        }
        private void UpdateThumbnails(List<SelectImages> images)
        {
            Thumbnails.ItemsSource = null;
            Thumbnails.Items.Clear();
            //images = SetResolutionAllImages(images);
            
           
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
                if (data.IsCheck)
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
