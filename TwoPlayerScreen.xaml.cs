using System.Windows;
using Tron_Mario.Models;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for TwoPlayerScreen.xaml
    /// </summary>
    public partial class TwoPlayerScreen : Window
    {
        public TwoPlayerScreen()
        {
            InitializeComponent();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            var playerInformation = new PlayerInformation()
            {
                PlayerNameOne = PlayerNameOne.Text,
                PlayerNameTwo = PlayerNameTwo.Text,
                Multiplayer = true,
                Score = 0
            };
            Level1 level1 = new Level1(true);
            level1.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }
    }
}
