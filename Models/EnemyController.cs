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
        private Dictionary<int, ImageBrush> Skins = new Dictionary<int, ImageBrush>();
        private Rectangle Enemy;
        private bool MoveLeft, MoveRight, Grounded;
        private int Speed = 4;
        private float Gravity = 10;

        public Rect Hitbox { get; private set; }

        /// <summary>
        /// controller object for an enemy
        /// </summary>
        /// <param name="enemy">enemy object</param>
        public EnemyController(Rectangle enemy)
        {
            Enemy = enemy;
            
            ImageBrush MeleeEnemySkinLeft = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/MeleeEnemyLeft.png"))};
            ImageBrush MeleeEnemySkinRight = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/MeleeEnemyRight.png"))};
            Skins.Add(0, MeleeEnemySkinLeft);
            Skins.Add(1, MeleeEnemySkinRight);
            Enemy.Fill = MeleeEnemySkinLeft;
        }

        /// <summary>
        /// Is called every time the gameEngine is called
        /// </summary>
        /// <param name="controller">playercontroller object</param>
        public void OnTick(PlayerController controller)
        {
            Hitbox = new Rect(Canvas.GetLeft(Enemy), Canvas.GetTop(Enemy), Enemy.Width, Enemy.Height);
            double DistanceToPlayer = controller.Hitbox.Left - Hitbox.Left;
            if (DistanceToPlayer < 0) DistanceToPlayer *= -1;

            // stop following if the enemy is no longer on screen
            if (!(DistanceToPlayer <= 960)) return;
            if (!Grounded) {
                Gravity += .3f;
                if (Gravity >= 10) Gravity = 10;
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