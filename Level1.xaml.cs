using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tron_Mario.Models;

namespace Tron_Mario
{
    public partial class Level1 : Window
    {
        private readonly List<EnemyController> Enemies = new List<EnemyController>();
        private readonly DispatcherTimer GameTimer = new DispatcherTimer();
        private readonly PlayerController Controller;

        public Level1()
        {
            InitializeComponent();

            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += GameEngine;
            GameTimer.Start();
            
            // call inside level initializer to create the player controller
            Controller = new PlayerController(Player, HealthMeter, GameCanvas, CameraStop);

            foreach (var x in GameCanvas.Children.OfType<Rectangle>()) {
                if ((string) x.Tag != "enemy") continue;
                var enemy = new EnemyController(x);
                Enemies.Add(enemy);
            }
            
            GameCanvas.Focus();
        }
        
        /// <summary>
        /// engine is called every 16 milliseconds
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void GameEngine(object sender, EventArgs e)
        {
            Controller.OnTick(Debug);

            // platform logic
            foreach (var x in GameCanvas.Children.OfType<Rectangle>()) {
                if ((string) x.Tag != "walkable") continue;

                var floorHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if (Controller.Hitbox.IntersectsWith(floorHitbox)) {
                    Controller.HandleLanding(floorHitbox);
                    break;
                } else Controller.Fall();
            }

            // enemy handling
            for (var i = 0; i <= Enemies.Count - 1; i++) {
                var enemy = Enemies[i];
                Debug.Content = "Dead";
                if (enemy.Dead) {
                    Enemies.Remove(enemy);
                    i--;
                    continue;
                }
                
                enemy.OnTick(Controller, GameCanvas);
                
                foreach (var x in GameCanvas.Children.OfType<Rectangle>()) {
                    if ((string) x.Tag != "walkable") continue;

                    var floorHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // damage player
                    if (Controller.Hitbox.IntersectsWith(enemy.Hitbox)) Controller.TakeDamage();
                    // handle landing and falling on/off platforms
                    if (enemy.Hitbox.IntersectsWith(floorHitbox)) {
                        enemy.HandleLanding(floorHitbox);
                        break;
                    } else enemy.Fall();
                }
            }
        }

        // movement
        /// <summary>
        /// fires when key is pressed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">KeyEventArgs</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Controller.OnKeyDown(e);
        }
        
        /// <summary>
        /// fires when key is let loose
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">KeyEventArgs</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Controller.OnKeyUp(e);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void Die(object sender, RoutedEventArgs e)
        {
            var death = new Death();
            death.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}