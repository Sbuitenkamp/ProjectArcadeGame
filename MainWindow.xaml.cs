using System.Windows;

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
        }

        private void OnePlayer(object sender, RoutedEventArgs e)
        {
            var onePlayerScreen = new OnePlayerScreen();
            onePlayerScreen.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void TwoPlayer(object sender, RoutedEventArgs e)
        {
            var twoPlayerScreen = new TwoPlayerScreen();
            twoPlayerScreen.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void EndGame(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenHighScores(object sender, RoutedEventArgs e)
        {
            var highscoreScreen = new HighScoreScreen();
            highscoreScreen.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }
    }
}
