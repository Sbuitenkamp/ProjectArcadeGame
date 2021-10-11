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

namespace Tron_Mario 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window 
    {
        public MainWindow() 
        {
            InitializeComponent();
            //Level1 level1 = new Level1();
            //level1.Visibility = Visibility.Visible;
            //this.Visibility = Visibility.Hidden;
        }



        private void OnePlayer(object sender, RoutedEventArgs e)
        {
            One_player_screen one_Player_Screen = new One_player_screen();
            one_Player_Screen.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void TwoPlayer(object sender, RoutedEventArgs e)
        {
            Two_Player_Screen Two_Player_Screen = new Two_Player_Screen();
            Two_Player_Screen.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void End_Game(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Open_Highsores(object sender, RoutedEventArgs e)
        {
            Highscore_screen Highscore_screen = new Highscore_screen();
            Highscore_screen.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }
    }
}
