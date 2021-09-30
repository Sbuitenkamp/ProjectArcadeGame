using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Tron_Mario
{
    public partial class Level1 : Window
    {
        // int = direction; 0=left, 1=right, (potentially 2=up and 3=down)
        private Dictionary<int, ImageBrush> PlayerSkins = new Dictionary<int, ImageBrush>();
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private bool MoveLeft, MoveRight;

        public Level1()
        {
            InitializeComponent();
            
            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += GameEngine;
            GameTimer.Start();
            // TODO make this shit work without crashing 

            ImageBrush playerSkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerRight.png"))};
            ImageBrush playerSkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerLeft.png"))};
            
            PlayerSkins.Add(0, playerSkinLeft);
            PlayerSkins.Add(1, playerSkinRight);
            
            Player.Fill = playerSkinRight;

            GameCanvas.Focus();
        }

        // engine; is called every 16 milliseconds
        private void GameEngine(object sender, EventArgs e)
        {
            if (MoveLeft) Canvas.SetLeft(Player, Canvas.GetLeft(Player) - 10);
            else if (MoveRight) Canvas.SetLeft(Player, Canvas.GetLeft(Player) + 10);
        }

        // movement
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) {
                case Key.A: //fallthrough
                case Key.Left:
                    MoveLeft = true;
                    Player.Fill = PlayerSkins[0];
                    break;
                case Key.D: //fallthrough
                case Key.Right:
                    MoveRight = true;
                    Player.Fill = PlayerSkins[1];
                    break;
            }
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key) {
                case Key.A: //fallthrough
                case Key.Left:
                    MoveLeft = false;
                    break;
                case Key.D: //fallthrough
                case Key.Right:
                    MoveRight = false;
                    break;
            }
        }
    }
}