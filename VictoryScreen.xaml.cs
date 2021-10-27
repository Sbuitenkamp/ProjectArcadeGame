using System.Windows;
using Tron_Mario.Models;

namespace Tron_Mario
{
    public partial class VictoryScreen : Window
    {
        public VictoryScreen(PlayerInformation playerInformation)
        {
            InitializeComponent();
            Score.Content = "Score: " + playerInformation.Score;
            PlayerName.Content = "Gefeliciteerd " + playerInformation.PlayerNameOne + (playerInformation.Multiplayer ? "&" + playerInformation.PlayerNameTwo : "") + "!";
        }
        
        private void MainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}