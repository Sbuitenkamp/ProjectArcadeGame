using System.Windows;

namespace Tron_Mario
{
    /// <summary>
    /// Interaction logic for Death.xaml
    /// </summary>
    public partial class Death : Window
    {
        public Death()
        {
            InitializeComponent();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
