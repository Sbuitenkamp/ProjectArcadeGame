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
            var sortedHighScoresSinglePlayer = from score in DatabaseHandler.HighScoresSinglePlayer select score;
            var sortedHighScoresMultiPlayer = from score in DatabaseHandler.HighScoresMultiPlayer select score;
            foreach (var highScoreSinglePlayer in sortedHighScoresSinglePlayer)
            {
                var label = new Label
                {
                    Content = highScoreSinglePlayer.Key + " scored " + highScoreSinglePlayer.Value,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                SinglePlayerHighScoresPanel.Children.Add(label);
            }
            foreach (var highScoreMultiPlayer in sortedHighScoresMultiPlayer)
            {
                var labels = new Label
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
            var mainMenu = new MainWindow();
            mainMenu.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
