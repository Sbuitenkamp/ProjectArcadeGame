using System.Windows;
using Tron_Mario.Models;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for Death.xaml
    /// </summary>
    public partial class Death : Window
    {
        private PlayerInformation PlayerInformation;

        public Death(PlayerInformation playerInformation)
        {
            InitializeComponent();
            PlayerInformation = playerInformation;
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
