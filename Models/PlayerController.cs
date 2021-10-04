using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tron_Mario.Models
{
    public class PlayerController
    {
        // int = direction; 0=left, 1=right, (potentially 2=up and 3=down)
        private Dictionary<int, ImageBrush> PlayerSkins = new Dictionary<int, ImageBrush>();
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private Rectangle Player;    
        private Canvas GameCanvas;    
        private bool MoveLeft, MoveRight;

        public PlayerController(Canvas gameCanvas, Rectangle player)
        {
            Player = player;
            GameCanvas = gameCanvas;

            ImageBrush playerSkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerRight.png"))};
            ImageBrush playerSkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerLeft.png"))};
            
            PlayerSkins.Add(0, playerSkinLeft);
            PlayerSkins.Add(1, playerSkinRight);
            
            Player.Fill = playerSkinRight;

            GameCanvas.Focus();
        }

        public void Move()
        {
            if (MoveLeft) Canvas.SetLeft(Player, Canvas.GetLeft(Player) - 10);
            else if (MoveRight) Canvas.SetLeft(Player, Canvas.GetLeft(Player) + 10);
        }

        // movement
        public void OnKeyDown(KeyEventArgs e)
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
        public void OnKeyUp(KeyEventArgs e)
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