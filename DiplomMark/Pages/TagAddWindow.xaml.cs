using Aspose.Html.Dom.Css;
using DiplomMark.Classes.HelpClasses;
using DiplomMark.Pages;
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
using System.Windows.Shapes;

namespace DiplomMark
{
    /// <summary>
    /// Логика взаимодействия для TagAddWindow.xaml
    /// </summary>
    public partial class TagAddWindow : Window
    {
        public TagAddWindow()
        {
            InitializeComponent();
            SelectColorTag.SelectedColor = Color.FromArgb(40, (byte)255, (byte)255, (byte)0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (String.IsNullOrEmpty(TagAddTB.Text) || SelectColorTag.SelectedColor.Value == null)
                return;
            Color myColor = SelectColorTag.SelectedColor.Value;
            TagClass tag = new TagClass { TagColor = new SolidColorBrush(Color.FromArgb(40, (byte)myColor.R, (byte)myColor.G, (byte)myColor.B)), TagName = TagAddTB.Text };
            GlobalVars.Tags.Add(tag);

            MainPage.MainPageController.ListBoxAllElements.ItemsSource = null;
            MainPage.MainPageController.ListBoxAllElements.ItemsSource = GlobalVars.Tags;
            MainPage.MainPageController.ListBoxAllElements.SelectedIndex = MainPage.MainPageController.ListBoxAllElements.Items.Count - 1;
            this.Close();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalVars.IsWindowTagOpen = false;
        }
    }
}
