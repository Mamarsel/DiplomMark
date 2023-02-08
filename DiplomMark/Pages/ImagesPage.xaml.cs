using DiplomMark.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        List<string> paths = new List<string>();
        List<BitmapImage> itemsList = new List<BitmapImage>();
        static List<SelectImages> images = new List<SelectImages>();
        List<SelectImages> Select_Images { get; set; }
        public ImagesPage()
        {
            InitializeComponent();
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\JutsPC\Desktop\Diplom\парсер2\фото");
            foreach (var fi in di.GetFiles())
            {
                images.Add(new SelectImages { FileURL = fi.FullName, isCheck = true });
            }
            for (int i = 0; i < images.Count; i++)
            {
                Thumbnails.Items.Add(images[i]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            images.RemoveAll(x => x.isCheck == false);
            MainWindow.main.MainFrame.Navigate(new MainPage(images));
        }

        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_item = Thumbnails.SelectedItem as SelectImages;
            if (selected_item == null)
                return;
            var selectedimage = images.FirstOrDefault(x => x.FileURL == selected_item.FileURL);
            var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(Thumbnails.SelectedIndex) as ListBoxItem;
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);

            if (myContentPresenter == null) { return; }
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            Image target = (Image)myDataTemplate.FindName("CBSelected", myContentPresenter);
            if (!selectedimage.isCheck)
            {
                target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/correct.png"));
                images.FirstOrDefault(x => x.FileURL == selected_item.FileURL).isCheck = true;
                Thumbnails.SelectedItem = null;
            }
            else if (selectedimage.isCheck)
            {
                target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/remove.png"));
                images.FirstOrDefault(x => x.FileURL == selected_item.FileURL).isCheck = false;
                Thumbnails.SelectedItem = null;
            }

        }
        private childItem FindVisualChild<childItem>(DependencyObject obj)
                 where childItem : DependencyObject
        {
            if (obj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
