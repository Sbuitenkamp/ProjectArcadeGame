using System;
using System.Collections.Generic;
using System.Windows;
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
        private Rectangle Player;
        private Canvas GameCanvas;
        private bool MoveLeft, MoveRight, Jumping, Grounded;
        private int Speed = 10;

        private Label Debug;

        public Rect PlayerHitbox { get; private set; }
        public float Gravity = 10;

        /// <summary>
        /// controller object for the player
        /// </summary>
        /// <param name="gameCanvas">canvas of the game</param>
        /// <param name="player">player object</param>
        public PlayerController(Canvas gameCanvas, Rectangle player)
        {
            Player = player;
            GameCanvas = gameCanvas;

            ImageBrush playerSkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerLeft.png"))};
            ImageBrush playerSkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerRight.png"))};
            PlayerSkins.Add(0, playerSkinLeft);
            PlayerSkins.Add(1, playerSkinRight);
            Player.Fill = playerSkinRight;

            GameCanvas.Focus();
        }

        /// <summary>
        /// is called every 16 milliseconds
        /// </summary>
        /// <param name="debug"></param>
        public void OnTick(Label debug)
        {
            Debug = debug;
//            Debug.Content = "Grounded: " + Grounded + ", Jumping: " + Jumping;
            // hitbox
            PlayerHitbox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
            
            // gravity
            Canvas.SetTop(Player, Canvas.GetTop(Player) + Gravity);

            if (Jumping) {
                Gravity += .5f;
                if (Gravity >= 10) {
                    Jumping = false;
                    Gravity = 10;
                }
            }
            // movement and create borders on the edge of the screen
            if (MoveLeft && Canvas.GetLeft(Player) > 0) Canvas.SetLeft(Player, Canvas.GetLeft(Player) - Speed);
            else if (MoveRight && Canvas.GetLeft(Player) + Player.Width < Application.Current.MainWindow.Width) Canvas.SetLeft(Player, Canvas.GetLeft(Player) + Speed);
        }

        // movement
        /// <summary>
        /// fires when key is pressed
        /// </summary>
        /// <param name="e">keyeventargs</param>
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
                case Key.W: // fallthrough
                case Key.Up:
                case Key.Space:
                    if (Jumping || !Grounded) break;
                    Gravity = -10;
                    Jumping = true;
                    Grounded = false;
                    break;
            }
        }
        /// <summary>
        /// fires when key is let loose
        /// </summary>
        /// <param name="e">keyeventargs</param>
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
        
        /// <summary>
        /// fires when player lands on a platform
        /// </summary>
        /// <param name="floor">the floor that the player landed on</param>
        public void HandleLanding(Rectangle floor)
        {
            if (Grounded) return;
            if (Jumping && Gravity < 0) return;
            Gravity = 0;
            Canvas.SetTop(Player, Canvas.GetTop(floor) - Player.Height);
            Grounded = true;
        }

        /// <summary>
        /// fires when player falls off a platform
        /// </summary>
        public void Fall()
        {
            if (Jumping) return;
            Gravity = 10;
            Grounded = false;
        }
    }
}