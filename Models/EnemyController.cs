using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tron_Mario.Models
{
    public class EnemyController
    {
        private readonly Dictionary<int, ImageBrush> Skins = new Dictionary<int, ImageBrush>();
        private bool MoveLeft, MoveRight, Grounded, Boss, Invincible;
        private const int Speed = 8;
        private int Health = 1;
        private float Gravity = 15;

        public Rect Hitbox { get; private set; }
        public Rectangle Enemy { get; }
        public bool Dead;

        /// <summary>
        /// controller object for an enemy
        /// </summary>
        /// <param name="enemy">enemy object</param>
        public EnemyController(Rectangle enemy)
        {
            Enemy = enemy;
            ImageBrush MeleeEnemySkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/EnemyLeft.png"))};
            ImageBrush MeleeEnemySkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/EnemyRight.png"))};
            Skins.Add(0, MeleeEnemySkinLeft);
            Skins.Add(1, MeleeEnemySkinRight);
            Enemy.Fill = MeleeEnemySkinLeft;
            // the boss is a bit beefier
            if (enemy.Name == "Boss") {
                Health = 2;
                Boss = true;
            }
        }

        /// <summary>
        /// Is called every time the gameEngine is called
        /// </summary>
        /// <param name="controller">player controller object</param>
        /// <param name="gameCanvas">game canvas object</param>
        public void OnTick(PlayerController controller, Canvas gameCanvas)
        {
            Hitbox = new Rect(Canvas.GetLeft(Enemy), Canvas.GetTop(Enemy), Enemy.Width, Enemy.Height);
            // stop following if the enemy is no longer on screen
            if (Canvas.GetLeft(Enemy) + Enemy.Width > Application.Current.MainWindow.Width || Canvas.GetLeft(Enemy) < 0) return;
            if (!Grounded) {
                Gravity += .3f;
                if (Gravity >= 15) Gravity = 15;
            } else Gravity = 0;

            Canvas.SetTop(Enemy, Canvas.GetTop(Enemy) + Gravity);

            if (MoveRight && Canvas.GetLeft(Enemy) + Enemy.Width < Application.Current.MainWindow.Width) {
                Canvas.SetLeft(Enemy, Canvas.GetLeft(Enemy) + Speed);
                Enemy.Fill = Skins[1];
            }
            if (MoveLeft && Canvas.GetLeft(Enemy) > 0) {
                Canvas.SetLeft(Enemy, Canvas.GetLeft(Enemy) - Speed);
                Enemy.Fill = Skins[0];
            }
            if (Hitbox.Left > controller.Hitbox.Left) {
                MoveLeft = true;
                MoveRight = false;
            }
            if (Hitbox.Left < controller.Hitbox.Left) {
                MoveRight = true;
                MoveLeft = false;
            }
            
            for (int i = 0; i < controller.PlayerProjecticles.Count; i++)
            {
                Bullet x = controller.PlayerProjecticles[i];
                Rect bulletHitbox = new Rect(Canvas.GetLeft(x.Projectile), Canvas.GetTop(x.Projectile), x.Projectile.Width, x.Projectile.Height);
                
                if (Hitbox.IntersectsWith(bulletHitbox)) {
                    if (Invincible) return;
                    Health--;
                    controller.PlayerProjecticles.Remove(x);
                    gameCanvas.Children.Remove(x.Projectile);
                    if (Health > 0) {
                        Invincible = true;
                        // set timeout to lift invincibility
                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                        CancellationToken cancellationToken = cancellationTokenSource.Token;
                        Task.Delay(300, cancellationToken).ContinueWith( t => {
                            Invincible = false;
                        }, cancellationToken);
                        return;
                    } 
                    gameCanvas.Children.Remove(Enemy);
                    Dead = true;
                    if (Boss) controller.BossKilled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Handles the landing of an individual enemy
        /// </summary>
        /// <param name="hitbox">hitbox of the landed platform</param>
        public void HandleLanding(Rect hitbox)
        {
            if (Grounded) return;
            if (Hitbox.Top + Hitbox.Height >= hitbox.Top + hitbox.Height * 0.5) return;
            Canvas.SetTop(Enemy, hitbox.Top - Enemy.Height);
            Grounded = true;
        }

        /// <summary>
        /// Handles the falling of an individual enemy
        /// </summary>
        public void Fall()
        {
            Gravity = 8;
            Grounded = false;
        }

        private void GoInvincible(object sender, EventArgs e)
        {
            
        }
    }
}