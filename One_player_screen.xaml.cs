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
    /// Interaction logic for One_player_screen.xaml
    /// </summary>
    public partial class One_player_screen : Window
    {
        public One_player_screen()
        {
            InitializeComponent();
        }

        private void Main_menu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }

        private void Start_game(object sender, RoutedEventArgs e)
        {
            Level1 Level1 = new Level1();
            Level1.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }
    }
}
