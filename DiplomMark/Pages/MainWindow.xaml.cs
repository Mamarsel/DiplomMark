using DiplomMark.Pages;
using System.Windows;
using System.Windows.Input;

namespace DiplomMark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow main = new MainWindow();
        
        public MainWindow()
        {
            InitializeComponent();
            main = this;
            MainFrame.Navigate(new WelcomePage());
        }
        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new WelcomePage());
        }

        private void Image_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage());
        }
    }
}
