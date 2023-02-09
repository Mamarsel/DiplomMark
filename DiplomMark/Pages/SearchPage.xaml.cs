using Microsoft.WindowsAPICodePack.Dialogs;

using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Windows;
using System.Windows.Controls;
using Python.Included;
using Python.Runtime;

using System.Diagnostics;
using Path = System.IO.Path;
using Microsoft.VisualBasic.ApplicationServices;
using Aspose.Html.Net;

using System;

using MessageBox = System.Windows.MessageBox;



using System.Windows.Documents;
using Aspose.Html.Dom;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using MS.WindowsAPICodePack.Internal;
using System.Drawing;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public static string catalog = "C:\\Users\\JutsPC\\Desktop\\Diplom\\парсер2\\фото\\"; // дефолтный каталог с фотографиями
        string ca = "C:\\Users\\JutsPC\\Desktop\\das";
        List<String> imageList = new List<string>();
        System.Windows.Forms.ImageList imageImageList = new System.Windows.Forms.ImageList();
        public SearchPage()
        {
            InitializeComponent();
        }
        dynamic json;

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTB.Text != "") 
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(catalog);
                if (di != null)
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
                                foreach (FileInfo file in di.GetFiles())
                                {
                                    file.Delete();
                                }
                                foreach (DirectoryInfo dir in di.GetDirectories())
                                {
                                    dir.Delete(true);
                                }
                                break;
                            }
                        case MessageBoxResult.No:
                            break;
                    }
                }
                string arg = string.Format(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName + "\\PythonScripts\\dist\\parse.exe {0} {1}", catalog, '"' + SearchTB.Text + '"');
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe ";
                startInfo.Arguments = @"/c " + arg; // cmd.exe spesific implementation
                p.StartInfo = startInfo;
                p.Start();
                p.WaitForExit();
            }







            MainWindow.main.MainFrame.Navigate(new MainPage());

        }

        private void CatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                catalog = dialog.FileName + "\\";
            }
        }
    }

}
