using Aspose.Html.Dom.Events;
using DiplomMark.Classes.DatabaseClasses;
using DiplomMark.Classes.HelpClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ApplicationContext = DiplomMark.Classes.DatabaseClasses.ApplicationContext;
using MessageBox = System.Windows.MessageBox;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        ApplicationContext db = new ApplicationContext();
        public WelcomePage()
        {
            InitializeComponent();
            db.Database.EnsureCreated();
            db.RecentProject.Load();
            DataContext = db.RecentProject.Local.ToObservableCollection();
            if(db.SettingsUser.FirstOrDefault() == null)
            {
                
                SettingsUser settings = new SettingsUser() { GuideViewAI = false, IdSetting = 1, PathToONNXFile = Directory.GetCurrentDirectory() + @"\Assets\Weights\yolov8m.onnx" };
                db.SettingsUser.Add(settings);
                db.SaveChanges();
            }
            UpdateLBAItem();
        }
        private void UpdateLBAItem()
        {
            ListBoxAllElements.ItemsSource = db.RecentProject.OrderByDescending(x=>x.LastModify).ToList();
        }
        private static string[] GetFilesFromDirectory(string catalog)
        {
            string[] FilesJpg = Directory.GetFiles(catalog, "*.jpg");
            string[] FilesPng = Directory.GetFiles(catalog, "*.png");
            return FilesJpg.Concat(FilesPng).ToArray();
        }
        private void LocalFolderGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog() { 
                InitialDirectory = GlobalVars.SelectedCatalog, 
                IsFolderPicker= true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GlobalVars.SelectedCatalog = dialog.FileName;
                RecentProjects r = new RecentProjects()
                {
                    FolderName = new DirectoryInfo(GlobalVars.SelectedCatalog).Name,
                    FullPath = GlobalVars.SelectedCatalog,
                    LastModify = DateTime.UtcNow
                };
                if (!db.RecentProject.Contains(r))
                {
                    db.RecentProject.Add(r);
                    db.SaveChanges();
                }
            }
            UpdateLBAItem();
        }
        /// <summary>
        /// Удаление каталога из вкладки "RecentProjects"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var _currentSelectedListBoxItem = ListBoxAllElements.ItemContainerGenerator.ContainerFromIndex(ListBoxAllElements.SelectedIndex) as ListBoxItem;
                if (_currentSelectedListBoxItem != null)
                {
                    RecentProjects myItem = (RecentProjects)_currentSelectedListBoxItem.DataContext;
                    db.RecentProject.Remove(myItem);
                    db.SaveChanges();
                    UpdateLBAItem();
                }
            }
            catch { }
            
        }
        private void ListBoxAllElements_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxAllElements.SelectedItem != null)
            {
                var _currentSelectedListBoxItem = ListBoxAllElements.ItemContainerGenerator.ContainerFromIndex(ListBoxAllElements.SelectedIndex) as ListBoxItem;
                RecentProjects myItem = (RecentProjects)_currentSelectedListBoxItem.DataContext;
                GlobalVars.SelectedCatalog = myItem.FullPath;
                var files = GetFilesFromDirectory(myItem.FullPath);
                using (var db = new ApplicationContext())
                {
                    var project = db.RecentProject.FirstOrDefault(x => x.FullPath == GlobalVars.SelectedCatalog);
                    project.LastModify = DateTime.Now;
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                    UpdateLBAItem();
                }
                if (files.Length == 0)
                {
                    MessageBox.Show("Нет фотографии в каталоге");
                    return;
                }
               
                MainWindow.main.MainFrame.Navigate(new MainPage(files));
               
            }
        }
    }
}
