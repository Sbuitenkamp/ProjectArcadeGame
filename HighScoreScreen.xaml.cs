using System.Collections.Generic;
using System.Linq;
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
            SinglePlayerHighScoresPanel.Children.Clear();
            MultiPlayerHighScoresPanel.Children.Clear();
            IEnumerable<KeyValuePair<string, int>> sortedHighScoresSinglePlayer = from score in DatabaseHandler.HighScoresSinglePlayer select score;
            IEnumerable<KeyValuePair<string, int>> sortedHighScoresMultiPlayer = from score in DatabaseHandler.HighScoresMultiPlayer select score;
            foreach (KeyValuePair<string, int> highScoreSinglePlayer in sortedHighScoresSinglePlayer)
            {
                Label label = new Label
                {
                    Content = highScoreSinglePlayer.Key + " scored " + highScoreSinglePlayer.Value,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                SinglePlayerHighScoresPanel.Children.Add(label);
            }
            foreach (KeyValuePair<string, int> highScoreMultiPlayer in sortedHighScoresMultiPlayer)
            {
                Label labels = new Label
                {
                    Content = highScoreMultiPlayer.Key + " scored " + highScoreMultiPlayer.Value,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
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
