using System;
using System.Collections.Generic;
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
        private readonly Rectangle Enemy;
        private bool MoveLeft, MoveRight, Grounded;
        private const int Speed = 8;
        private float Gravity = 15;

        public Rect Hitbox { get; private set; }
        public bool Dead;

        /// <summary>
        /// controller object for an enemy
        /// </summary>
        /// <param name="enemy">enemy object</param>
        public EnemyController(Rectangle enemy)
        {
            Enemy = enemy;
            var MeleeEnemySkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/EnemyLeft.png"))};
            var MeleeEnemySkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/EnemyRight.png"))};
            Skins.Add(0, MeleeEnemySkinLeft);
            Skins.Add(1, MeleeEnemySkinRight);
            Enemy.Fill = MeleeEnemySkinLeft;
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
            
            for (var i = 0; i < controller.PlayerProjecticles.Count; i++)
            {
                var x = controller.PlayerProjecticles[i];
                var bulletHitbox = new Rect(Canvas.GetLeft(x.Projectile), Canvas.GetTop(x.Projectile), x.Projectile.Width, x.Projectile.Height);
                if (Hitbox.IntersectsWith(bulletHitbox))
                {
                    gameCanvas.Children.Remove(Enemy);
                    gameCanvas.Children.Remove(x.Projectile);
                    controller.PlayerProjecticles.Remove(x);
                    Dead = true;
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
    }
}