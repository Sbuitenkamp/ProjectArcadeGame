using System.Windows;

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
            var mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            var level1 = new Level1();
            level1.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }
    }
}
