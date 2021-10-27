using System.Windows;
using Tron_Mario.Models;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for TwoPlayerDeathScreen.xaml
    /// </summary>
    public partial class TwoPlayerDeathScreen : Window
    {
        private PlayerInformation PlayerInformation;
        public TwoPlayerDeathScreen(PlayerInformation playerInformation)
        {
            InitializeComponent();
            PlayerInformation = playerInformation;
            playerInformation.PlayerOneHasPlayed = !playerInformation.PlayerOneHasPlayed; // invert the boolean that checks if player one has played
            PlayerToPlay.Content = (playerInformation.PlayerOneHasPlayed ? playerInformation.PlayerNameTwo : playerInformation.PlayerNameOne) + " is nu aan de beurt.";
            Score.Content = "Score: " + playerInformation.Score;
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Respawn(object sender, RoutedEventArgs e)
        {
            Level1 level1 = new Level1(PlayerInformation);
            level1.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
