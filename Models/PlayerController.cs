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
        private readonly Dictionary<string, ImageBrush> PlayerSkins = new Dictionary<string, ImageBrush>();
        private readonly Dictionary<int, ImageBrush> HealthIndicators = new Dictionary<int, ImageBrush>();
        private readonly DispatcherTimer InvincibleTimer = new DispatcherTimer();
        private Brush LastPlayerSkin;
        private readonly Canvas GameCanvas;
        private readonly Rectangle Player, HealthIndicator, CameraStopLeft, CameraStopRight;
        private bool MoveLeft, MoveRight, Jumping, Grounded, Invincible, Visible = true, FreeMovement, LeftStopSpawned, RightStopSpawned, Shooting, FacingRight = true;
        private float Gravity = 15;
        private const int Speed = 15;
        private int Health;

        private Label Debug;

        public bool TwoPlayer;
        public readonly List<Bullet> PlayerProjecticles = new List<Bullet>();
        public Rect Hitbox { get; private set; }

        /// <summary>
        /// controller object for the player
        /// </summary>
        /// <param name="player">player object</param>
        /// <param name="healthIndicator">health meter object</param>
        /// <param name="gameCanvas">canvas of the game</param>
        /// <param name="cameraStopLeft">rectangle that notifies when the camera is to stop following</param>
        /// <param name="cameraStopRight">rectangle that notifies when the camera is to stop following</param>
        public PlayerController(Rectangle player, Rectangle healthIndicator, Canvas gameCanvas, Rectangle cameraStopLeft, Rectangle cameraStopRight)
        {
            Player = player;
            HealthIndicator = healthIndicator;
            GameCanvas = gameCanvas;
            CameraStopLeft = cameraStopLeft;
            CameraStopRight = cameraStopRight;

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
            Rect cameraStopLeftHitbox = new Rect(Canvas.GetLeft(CameraStopLeft), Canvas.GetTop(CameraStopLeft), CameraStopLeft.Width, CameraStopLeft.Height);
            Rect cameraStopRightHitbox = new Rect(Canvas.GetLeft(CameraStopRight), Canvas.GetTop(CameraStopRight), CameraStopRight.Width, CameraStopRight.Height);

            bool leftOfStop = LeftStopSpawned && Hitbox.Left <= cameraStopLeftHitbox.Left;
            bool rightOfStop = RightStopSpawned && Hitbox.Left + Hitbox.Width >= cameraStopRightHitbox.Left;

            Debug.Content = "Left: " + cameraStopLeftHitbox.Left + "\nRight: " + cameraStopRightHitbox.Left;

            // free the camera
            if (Hitbox.IntersectsWith(cameraStopLeftHitbox) || Hitbox.IntersectsWith(cameraStopRightHitbox) || (leftOfStop && !rightOfStop) || (!leftOfStop && rightOfStop)) FreeMovement = true;
            else {
                FreeMovement = false;
                if (RightStopSpawned) {
                    Canvas.SetLeft(CameraStopRight, 1100);
                    RightStopSpawned = false;
                }
                if (LeftStopSpawned) {
                    Canvas.SetLeft(CameraStopLeft, 0 - CameraStopLeft.Width);
                    LeftStopSpawned = false;
                }
                Canvas.SetLeft(Player, 939);
            }
            
            // gravity
            Canvas.SetTop(Player, Canvas.GetTop(Player) + Gravity);

            if (Jumping) {
                Gravity += .5f;
                if (Gravity >= 10) {
                    Jumping = false;
                    Gravity = 20;
                }
            } else if (Grounded) Gravity = 0;
            // movement and create borders on the edge of the screen
            if (MoveLeft) {
                if (Visible) Player.Fill = PlayerSkins["left"];
                if (FreeMovement) {
                    if (Canvas.GetLeft(Player) > 1) Canvas.SetLeft(Player, Canvas.GetLeft(Player) - Speed);
                } else Move(Speed);
            } else if (MoveRight) {
                if (Visible) Player.Fill = PlayerSkins["right"];
                if (FreeMovement) {
                    if (Canvas.GetLeft(Player) + Player.Width < Application.Current.MainWindow.Width ) Canvas.SetLeft(Player, Canvas.GetLeft(Player) + Speed);
                }
                else Move(-Speed);
            }

            for (int i = 0; i < PlayerProjecticles.Count; i++)
            {
                Bullet x = PlayerProjecticles[i];
                int speed = x.FacingRight ? 25 : -25;
                Canvas.SetLeft(x.Projectile, Canvas.GetLeft(x.Projectile) + speed);
                double bulletLeft = Canvas.GetLeft(x.Projectile);
                if (bulletLeft < 0 || bulletLeft > Application.Current.MainWindow.Width)
                {
                    GameCanvas.Children.Remove(x.Projectile);
                    PlayerProjecticles.Remove(x);
                    i--; // i - 1 to compensate the removal of the object from the collection of bullets
                }
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
        /// <param name="e">KeyEventArgs</param>
        public void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key) {
                case Key.A: //fallthrough
                case Key.Left:
                    MoveLeft = true;
                    MoveRight = false;
                    FacingRight = false;
                    break;
                case Key.D: //fallthrough
                case Key.Right:
                    MoveRight = true;
                    MoveLeft = false;
                    FacingRight = true;
                    break;
                case Key.W: // fallthrough
                case Key.Up:
                case Key.Space:
                    if (Jumping || !Grounded) break;
                    Gravity = -15;
                    Jumping = true;
                    Grounded = false;
                    break;
                case Key.F:
                    Shoot();
                    Shooting = true;
                    break;
            }
        }
        
        /// <summary>
        /// fires when key is let loose
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
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
                case Key.F:
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;
                    Task.Delay(500, cancellationToken).ContinueWith( t => Shooting = false, cancellationToken);
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
        public void TakeDamage(PlayerInformation playerInformation)
        {
            //This is just a test bool 
            //twoPlayer = true;
            if (Invincible) return;
            Health--;
            Invincible = true;
            if (Health <= 0) {
                if (playerInformation.Multiplayer) {
                    TwoPlayerDeathScreen twoPlayerDeathScreen = new TwoPlayerDeathScreen(playerInformation);
                    twoPlayerDeathScreen.Visibility = Visibility.Visible;
                    
                } else {
                    Death death = new Death(playerInformation);
                    death.Visibility = Visibility.Visible;
                }
                return;
            }
            HealthIndicator.Width -= 29;
            HealthIndicator.Fill = HealthIndicators[Health];

            // set timeout to lift invincibility
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            Task.Delay(3000, cancellationToken).ContinueWith( t => {
                Visible = true;
                Invincible = false;
            }, cancellationToken);
        }

        public void MoveFree(bool goRight)
        {
            // create the camera stops
            if (goRight) {
                CameraStopRight.Height = 795;
                CameraStopRight.Width = 34;
                Canvas.SetLeft(CameraStopRight, 981);
                Canvas.SetTop(CameraStopRight, 0);
                RightStopSpawned = true;
            } else {
                CameraStopLeft.Height = 795;
                CameraStopLeft.Width = 34;
                Canvas.SetLeft(CameraStopLeft, 905);
                Canvas.SetTop(CameraStopLeft, 0);
                LeftStopSpawned = true;
            }
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
            foreach (Rectangle x in GameCanvas.Children.OfType<Rectangle>()) {
                if (x.Name == "Player" || x.Name == "Floor" || x.Name == "HealthMeter") continue;
                Canvas.SetLeft(x, Canvas.GetLeft(x) + speed);
            }
            foreach (Image x in GameCanvas.Children.OfType<Image>()) {
                if ((string) x.Tag != "movable") continue;
                Canvas.SetLeft(x, Canvas.GetLeft(x) + speed);
            }
        }
        
        private void InitiateBullet()
        {
            Rectangle newBullet = new Rectangle
            {
                Tag = "bullet",
                Height = 5,
                Width = 20,
                Fill = Brushes.WhiteSmoke,
                Stroke = Brushes.Red
            };
            Canvas.SetLeft(newBullet, FacingRight ? Hitbox.Left + Hitbox.Width : Hitbox.Left - 20);
            Canvas.SetTop(newBullet, Canvas.GetTop(Player) + Player.Height / 2);

            GameCanvas.Children.Add(newBullet);
            Bullet bullet = new Bullet()
            {
                FacingRight = FacingRight,
                Projectile = newBullet
            };
            PlayerProjecticles.Add(bullet);
        }
         // if left is kleiner dan 0 / left of width > current main window
        private void Shoot()
        {
            if (Shooting) return;
            InitiateBullet();
        }
    }
}