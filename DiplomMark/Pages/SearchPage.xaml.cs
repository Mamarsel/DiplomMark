using Microsoft.WindowsAPICodePack.Dialogs;

using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Windows;
using System.Windows.Controls;
using Python.Included;
using Python.Runtime;
using System.Windows.Forms;
using System.Diagnostics;
using Path = System.IO.Path;
using Microsoft.VisualBasic.ApplicationServices;
using Aspose.Html.Net;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System;

using MessageBox = System.Windows.MessageBox;
using Numpy;
using static Numpy.np;
using static IronPython.Modules._ast;
using static Community.CsharpSqlite.Sqlite3;
using System.Windows.Documents;
using Aspose.Html.Dom;

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public static string catalog = "C:\\Users\\JutsPC\\Desktop\\Diplom\\парсер2\\фото"; // дефолтный каталог с фотографиями
        string ca = "C:\\Users\\JutsPC\\Desktop\\das";
        List<String> imageList = new List<string>();
        ImageList imageImageList = new ImageList();
        public SearchPage()
        {
            InitializeComponent();
        }
        dynamic json;

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            string arg = string.Format(@"C:\Users\JutsPC\Desktop\Демо-Экзамен\Rakhimov_Marcel\DiplomMark\DiplomMark\PythonScripts\dist\parse.exe {0} {1}", catalog , SearchTB.Text);
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe ";
            startInfo.Arguments = @"/c " + arg; // cmd.exe spesific implementation
            p.StartInfo = startInfo;
            p.Start();
            p.WaitForExit();
            MainWindow.main.MainFrame.Navigate(new MainPage());

        }

        private void CatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                catalog = dialog.FileName;
            }
        }

        private string GetHtmlCode()
        {

            string url = "https://www.google.com/search?q=" + "helloworld" + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }
        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }

        private byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000000);

                    return bytes;
                }
            }

            return null;
        }

    }

}
