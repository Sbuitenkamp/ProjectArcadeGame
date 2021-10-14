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
        private bool MoveLeft, MoveRight, Jumping, Grounded;
        private int Speed = 10;
        
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
        public void OnTick()
        {
            
        }
    }
}