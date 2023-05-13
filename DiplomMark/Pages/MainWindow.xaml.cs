
using DiplomMark.Pages;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
