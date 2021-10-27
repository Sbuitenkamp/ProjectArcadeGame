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
    /// Interaction logic for TwoPlayerDeathScreen.xaml
    /// </summary>
    public partial class TwoPlayerDeathScreen : Window
    {
        public TwoPlayerDeathScreen()
        {
            InitializeComponent();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Respawn(object sender, RoutedEventArgs e)
        {
            Level1 level1 = new Level1(true);
            level1.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
