using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Tron_Mario.Models;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for HighScoreScreen.xaml
    /// </summary>
    public partial class HighScoreScreen : Window
    {
        private DatabaseHandler DatabaseHandler = new DatabaseHandler();
        public HighScoreScreen()
        {
            InitializeComponent();
            DatabaseHandler.GetHighScoresSinglePlayer();
            DatabaseHandler.GetHighScoresMultiPlayer();
            CreateLabels();
        }
        
        private void CreateLabels()
        {
            // Clears the single player panel so that the scores can be added.
            SinglePlayerHighScoresPanel.Children.Clear();
            // Clears the multiplayer player panel so that the scores can be added.
            MultiPlayerHighScoresPanel.Children.Clear();
            
            // Loops through the list of scores from all the single players that are saved in the database
            foreach (KeyValuePair<string, int> highScoreSinglePlayer in DatabaseHandler.HighScoresSinglePlayer)
            {
                // Makes a new label that can be used to adds a label with the name and the score of the players
                Label label = new Label
                {
                    Content = highScoreSinglePlayer.Key + " scored " + highScoreSinglePlayer.Value,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                // The single player panel is automatically filled with the player names and their score
                SinglePlayerHighScoresPanel.Children.Add(label);
            }
            
            // Loops through the list of name and scores from all the multiplayer's that are saved in the database
            foreach (KeyValuePair<string, int> highScoreMultiPlayer in DatabaseHandler.HighScoresMultiPlayer)
            {
                // Makes a new label that can be used to adds a label with the name and the score of the players
                Label labels = new Label
                {
                    Content = highScoreMultiPlayer.Key + " scored " + highScoreMultiPlayer.Value,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };                
                // The multiplayer panel is automatically filled with the player names and their score
                MultiPlayerHighScoresPanel.Children.Add(labels);

            }
        }


        private void MainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainMenu = new MainWindow();
            mainMenu.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
