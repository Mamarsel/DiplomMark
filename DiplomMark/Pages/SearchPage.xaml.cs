using Microsoft.WindowsAPICodePack.Dialogs;

using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System;
using MessageBox = System.Windows.MessageBox;
using System.Reflection;
using System.Windows.Shapes;
using System.Linq;
using System.Security.Permissions;
using DiplomMark.Classes;
using Path = System.IO.Path;
using Medallion.Shell;
using System.Threading.Tasks;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public static string catalog = Directory.GetCurrentDirectory() + "\\Photos\\"; // дефолтный каталог с фотографиями
       
        public SearchPage()
        {
            InitializeComponent();
            
        }
      

        private void DeleteAllPhotosInDirectory()
        {
            
            DirectoryInfo di = new DirectoryInfo(catalog);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }


        private static string[] GetFilesFromDirectory()
        {
            string[] FilesJpg = Directory.GetFiles(catalog, "*.jpg");
            string[] FilesPng = Directory.GetFiles(catalog, "*.png");
            return   FilesJpg.Concat(FilesPng).ToArray();
        }

        private async Task RunShell()
        {
            
            var command = Command.Run(Directory.GetCurrentDirectory() + "\\PythonScripts\\dist\\parse.exe", catalog, '"' + SearchTB.Text + '"');
        
            await command.Task;
       
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var files = GetFilesFromDirectory();
                if (SearchTB.Text != "")
                {
                    if (files.Length > 0)
                    {
                        MessageBoxButton button = MessageBoxButton.YesNoCancel;
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        var result = MessageBox.Show("Удалить фотографии из этого каталога?", "Удаление фотографии", button, icon, MessageBoxResult.Yes);
                        switch (result)
                        {
                            case MessageBoxResult.Cancel:
                                break;
                            case MessageBoxResult.Yes:
                                {
                                    DeleteAllPhotosInDirectory();
                                    break;
                                }
                            case MessageBoxResult.No:
                                break;
                        }
                    }
                    ProgressGrid.Visibility = Visibility.Visible;
                    BeginBTN.IsEnabled = false;
                    await RunShell();




                }
                files = GetFilesFromDirectory();
                if (files.Length == 0)
                {
                    MessageBox.Show("Нет фотографии в каталоге. Загрузите либо выполните запрос");
                    return;
                }
                MainWindow.main.MainFrame.Navigate(new ImagesPage());
            }
           catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        private void CatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = catalog;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                catalog = dialog.FileName + "\\";
            }
        }
    }

}
