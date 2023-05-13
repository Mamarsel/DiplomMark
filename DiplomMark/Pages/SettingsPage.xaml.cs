using DiplomMark.Classes.DatabaseClasses;
using DiplomMark.Classes.HelpClasses;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DiplomMark.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            using (var db = new ApplicationContext())
            {
                var x = db.SettingsUser.First();
                OnnxTB.Text = x.PathToONNXFile;
            }
           
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CommonFileDialogFilter filter = new CommonFileDialogFilter("ONNX File", "*.onnx");
            CommonOpenFileDialog dialog = new CommonOpenFileDialog()
            {
                InitialDirectory = GlobalVars.SelectedCatalog,
                IsFolderPicker = false,
                Title = "Select ONNX folder",
                Multiselect = false,
            };
            dialog.Filters.Add(filter);
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            { 
               OnnxTB.Text = dialog.FileName;
               using(var db = new ApplicationContext())
               {
                    var x = db.SettingsUser.FirstOrDefault(x => x.IdSetting == 1);
                    x.PathToONNXFile = dialog.FileName;
                    db.SaveChanges();
               }
            }

        }
    }
}
