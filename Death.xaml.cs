using System.Windows;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for Death.xaml
    /// </summary>
    public partial class Death : Window
    {
        private bool MultiPlayer;

        public Death(bool multiPlayer)
        {
            InitializeComponent();
            MultiPlayer = multiPlayer;
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Respawn(object sender, RoutedEventArgs e)
        {
            var level1 = new Level1(MultiPlayer);
            level1.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
