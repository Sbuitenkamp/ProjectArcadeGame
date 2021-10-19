using System.Windows;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for HighScoreScreen.xaml
    /// </summary>
    public partial class HighScoreScreen : Window
    {
        public HighScoreScreen()
        {
            InitializeComponent();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            var mainMenu = new MainWindow();
            mainMenu.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
