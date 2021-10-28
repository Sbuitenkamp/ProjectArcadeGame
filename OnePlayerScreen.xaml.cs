using System.Windows;
using Tron_Mario.Models;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for OnePlayerScreen.xaml
    /// </summary>
    public partial class OnePlayerScreen : Window
    {
        public OnePlayerScreen()
        {
            InitializeComponent();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            PlayerInformation playerInformation = new PlayerInformation
            {
                PlayerNameOne = PlayerName.Text
            };
            Level1 level1 = new Level1(playerInformation);
            level1.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;

        }
    }
}
