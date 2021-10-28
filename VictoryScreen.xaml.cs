using System.Windows;
using Tron_Mario.Models;

namespace Tron_Mario
{
    public partial class VictoryScreen : Window
    {
        private PlayerInformation PlayerInformation;
        private DatabaseHandler DatabaseHandler = new DatabaseHandler();
        
        public VictoryScreen(PlayerInformation playerInformation)
        {
            InitializeComponent();
            PlayerInformation = playerInformation;
            Score.Content = "Score: " + playerInformation.Score;
            PlayerName.Content = playerInformation.PlayerNameOne + (playerInformation.Multiplayer ? "&" + playerInformation.PlayerNameTwo : "") + "!";
        }
        
        private void MainMenu(object sender, RoutedEventArgs e)
        {
            // save the score
            if (PlayerInformation.Multiplayer) DatabaseHandler.SetHighScoreMultiPlayer(PlayerInformation.PlayerNameOne, PlayerInformation.PlayerNameTwo, PlayerInformation.Score);
            else DatabaseHandler.SetHighScoreSinglePlayer(PlayerInformation.PlayerNameOne, PlayerInformation.Score);
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}