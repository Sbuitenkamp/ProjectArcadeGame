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

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for Highscore_screen.xaml
    /// </summary>
    public partial class Highscore_screen : Window
    {
        public Highscore_screen()
        {
            InitializeComponent();
        }

        private void Main_menu(object sender, RoutedEventArgs e)
        {
            MainWindow Main_Menu = new MainWindow();
            Main_Menu.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
