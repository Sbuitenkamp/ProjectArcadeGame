using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tron_Mario.Models
{
    public class PlayerController
    {
        // int = direction; 0=left, 1=right, (potentially 2=up and 3=down)
        private Dictionary<int, ImageBrush> PlayerSkins = new Dictionary<int, ImageBrush>();
        private Canvas GameCanvas;
        private Rectangle Player;
        private bool MoveLeft, MoveRight, Jumping, Grounded;
        private int Speed = 10;
        private float Gravity = 10;

        private Label Debug;

        public Rect Hitbox { get; private set; }

        /// <summary>
        /// controller object for the player
        /// </summary>
        /// <param name="player">player object</param>
        public PlayerController(Rectangle player)
        {
            Player = player;

            ImageBrush playerSkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerLeft.png"))};
            ImageBrush playerSkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerRight.png"))};
            PlayerSkins.Add(0, playerSkinLeft);
            PlayerSkins.Add(1, playerSkinRight);
            Player.Fill = playerSkinRight;

        }

        /// <summary>
        /// Is called every time the gameEngine is called
        /// </summary>
        /// <param name="debug"></param>
        public void OnTick(Label debug)
        {
            Debug = debug;
            // hitbox
            Hitbox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
            
            // gravity
            Canvas.SetTop(Player, Canvas.GetTop(Player) + Gravity);

            if (Jumping) {
                Gravity += .5f;
                if (Gravity >= 10) {
                    Jumping = false;
                    Gravity = 10;
                }
            } else if (Grounded) Gravity = 0;
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
        /// <param name="hitbox">the hitbox of floor that the player landed on</param>
        public void HandleLanding(Rect hitbox)
        {
            if (Grounded) return;
            Debug.Content = "Playertop: " + (Hitbox.Top + Hitbox.Height) + "\nPlatformTop: " + (hitbox.Top + hitbox.Height * 0.5);
            if (Jumping && Gravity < 0) return;
            // check if player is at least higher than the platform
            if (Hitbox.Top + Hitbox.Height >= hitbox.Top + hitbox.Height * 0.5) return;
            Canvas.SetTop(Player, hitbox.Top - Player.Height);
            Grounded = true;
            Jumping = false;
        }

        /// <summary>
        /// fires when player falls off a platform
        /// </summary>
        /// /// <param name="hitbox">the hitbox of the floor that the player is currently moving on</param>
        public void Fall()
        {
            // only fall if we're in the air and not jumping
            if (Jumping) return;
            Gravity = 5;
            Jumping = true;
            Grounded = false;
        }
    }
}