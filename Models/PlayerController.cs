using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private Dictionary<string, ImageBrush> PlayerSkins = new Dictionary<string, ImageBrush>();
        private Dictionary<int, ImageBrush> HealthIndicators = new Dictionary<int, ImageBrush>();
        private DispatcherTimer InvincibleTimer = new DispatcherTimer();
        private Brush LastPlayerSkin;
        private readonly Canvas GameCanvas;
        private readonly Rectangle Player, HealthIndicator, CameraStopLeft, CameraStopRight;
        private bool MoveLeft, MoveRight, Jumping, Grounded, Invincible, Visible = true, freeMovement;
        private float Gravity = 10;
        private const int Speed = 10;

        private Label Debug;

        public Rect Hitbox { get; private set; }
        public int Health { get; set; }

        /// <summary>
        /// controller object for the player
        /// </summary>
        /// <param name="player">player object</param>
        /// <param name="healthIndicator">health meter object</param>
        /// <param name="gameCanvas">canvas of the game</param>
        /// <param name="cameraStopLeft">rectangle that holds the camera back</param>
        public PlayerController(Rectangle player, Rectangle healthIndicator, Canvas gameCanvas, Rectangle cameraStopLeft)
        {
            Player = player;
            HealthIndicator = healthIndicator;
            GameCanvas = gameCanvas;
            CameraStopLeft = cameraStopLeft;

            // player skin index 
            ImageBrush playerSkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerLeft.png"))};
            ImageBrush playerSkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PlayerRight.png"))};
            PlayerSkins.Add("left", playerSkinLeft);
            PlayerSkins.Add("right", playerSkinRight);
            Player.Fill = playerSkinRight;

            // health meter
            Health = 3;
            ImageBrush oneHp = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/1hp.png"))};
            ImageBrush twoHp = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/2hp.png"))};
            ImageBrush threeHp = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/3hp.png"))};
            HealthIndicators.Add(1, oneHp);
            HealthIndicators.Add(2, twoHp);
            HealthIndicators.Add(3, threeHp);
            HealthIndicator.Fill = HealthIndicators[Health];

            InvincibleTimer.Interval = TimeSpan.FromMilliseconds(100);
            InvincibleTimer.Tick += ShowInvincible;
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
            Rect CameraStopLeftHitbox = new Rect(Canvas.GetLeft(CameraStopLeft), Canvas.GetTop(CameraStopLeft), CameraStopLeft.Width, CameraStopLeft.Height);

            if (Hitbox.IntersectsWith(CameraStopLeftHitbox)) freeMovement = true;
            else if (Hitbox.Left <= CameraStopLeftHitbox.Left) freeMovement = true;
            else freeMovement = false;

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
            if (MoveLeft) {
                if (Visible) Player.Fill = PlayerSkins["left"];
                if (freeMovement) {
                    if (Canvas.GetLeft(Player) > 1) Canvas.SetLeft(Player, Canvas.GetLeft(Player) - Speed);
                } else Move(Speed);
            } else if (MoveRight) {
                if (Visible) Player.Fill = PlayerSkins["right"];
                if (freeMovement) {
                    if (Canvas.GetLeft(Player) + Player.Width < Application.Current.MainWindow.Width) Canvas.SetLeft(Player, Canvas.GetLeft(Player) + Speed);
                } else Move(-Speed);
            }

            // feedback to the player that they're invincible for some time
            switch (Invincible) {
                case true when !InvincibleTimer.IsEnabled:
                    InvincibleTimer.Start();
                    break;
                case false when InvincibleTimer.IsEnabled:
                    InvincibleTimer.Stop();
                    Player.Fill = LastPlayerSkin;
                    break;
            }
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
                    MoveRight = false;
                    break;
                case Key.D: //fallthrough
                case Key.Right:
                    MoveRight = true;
                    MoveLeft = false;
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
            if (Jumping && Gravity < 0) return;
            // check if player is at least higher than the platform
            // we take a little bit of a buffer for checking the top because collision is detected after the player has already passed his feet through the platform
            if (Hitbox.Top + Hitbox.Height >= hitbox.Top + hitbox.Height * 0.75) return;
            Canvas.SetTop(Player, hitbox.Top - Player.Height);
            Grounded = true;
            Jumping = false;
        }

        /// <summary>
        /// fires when player falls off a platform
        /// </summary>
        public void Fall()
        {
            // only fall if we're in the air and not jumping
            if (Jumping) return;
            Gravity = 5;
            Jumping = true;
            Grounded = false;
        }

        /// <summary>
        /// fires when the player takes damage
        /// </summary>
        public void TakeDamage()
        {
            if (Invincible) return;
            Health--;
            Invincible = true;
            if (Health <= 0) {
                // TODO: death logic here
                return;
            }
            HealthIndicator.Width -= 29;
            HealthIndicator.Fill = HealthIndicators[Health];

            // set timeout to lift invincibility
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Delay(3000).ContinueWith(async (t) => Invincible = false, cancellationToken);
        }

        private void ShowInvincible(object sender, EventArgs e)
        {
            if (Visible) {
                LastPlayerSkin = Player.Fill;
                Player.Fill = new SolidColorBrush(Colors.Transparent);
                Visible = false;
            } else {
                Player.Fill = MoveLeft ? PlayerSkins["left"] : MoveRight ? PlayerSkins["right"] : LastPlayerSkin;
                Visible = true;
            }
        }

        /// <summary>
        /// move the whole canvas to create the illusion of the camera being focussed on the player.
        /// </summary>
        /// <param name="speed">movement speed, make it negative to go the other direction</param>
        private void Move(int speed)
        {
            // fuck it, we're moving the whole lot
            foreach (var x in GameCanvas.Children.OfType<Rectangle>()) {
                if (x.Name == "Player" || x.Name == "Floor" || x.Name == "HealthMeter") continue;
                Canvas.SetLeft(x, Canvas.GetLeft(x) + speed);
            }
            foreach (var x in GameCanvas.Children.OfType<Image>()) {
                if ((string) x.Tag != "movable") continue;
                Canvas.SetLeft(x, Canvas.GetLeft(x) + speed);
            }
        }
    }
}