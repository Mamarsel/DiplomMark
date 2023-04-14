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
using System.Security.Policy;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public static string SelectedCatalogs = GlobalVars.SelectedCatalog; // дефолтный каталог с фотографиями
        private string[] _existFiles;
       
        public SearchPage(string[] paths)
        {
            InitializeComponent();
            _existFiles = paths;
        }
      
        private static string[] GetFilesFromDirectory()
        {
            string[] FilesJpg = Directory.GetFiles(SelectedCatalogs, "*.jpg");
            string[] FilesPng = Directory.GetFiles(SelectedCatalogs, "*.png");
            return   FilesJpg.Concat(FilesPng).ToArray();
        }

        private async Task RunShell()
        {
            var command = Command.Run(Directory.GetCurrentDirectory() + "\\PythonScripts\\dist\\parse.exe", SelectedCatalogs, '"' + SearchTB.Text + '"');
            await command.Task;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
                // Получение списка файлов из выбранной директории
                var files = GetFilesFromDirectory();
               

                // Если в поле поиска введен текст
                if (SearchTB.Text != "")
                {
                    // Отображаем элементы UI для отображения прогресса выполнения операции
                    ProgressGrid.Visibility = Visibility.Visible;
                    BeginBTN.IsEnabled = false;
                    // Запускаем выполнение командной строки в асинхронном режиме
                    await RunShell();
                }
                // Получаем список файлов из директории
                files = GetFilesFromDirectory();
                // Если список файлов пустой, выводим сообщение об этом
                if (files.Length == 0)
                {
                    MessageBox.Show("Нет фотографии в каталоге. Загрузите либо выполните запрос");
                    return;
                }
            // Переходим на страницу с изображениями

            files = files.Where(x => !_existFiles.Contains(x)).ToArray();


            MainWindow.main.MainFrame.Navigate(new ImagesPage(files));
            
           

        }
    }

}
