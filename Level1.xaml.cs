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
        private readonly PlayerController PlayerController;
        private readonly PlatformHandler PlatformHandler;
        private PlayerInformation PlayerInformation;
        private bool MultiPlayer;

        public Level1(PlayerInformation playerInformation)
        {
            InitializeComponent();

            PlayerInformation = playerInformation;
            PlayerInformation.Score = 0;

            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += GameEngine;
            GameTimer.Start();
            
            // call inside level initializer to create the player controller
            PlayerController = new PlayerController(Player, HealthMeter, GameCanvas, CameraStopLeft, CameraStopRight);

            // a list of height coordinates for the platform generation
            List<double> platformHeights = new List<double> { 700, 640, 580, 680, 620, 560 };
            // call inside level initializer to create the platform controller
            PlatformHandler = new PlatformHandler(GameCanvas, PlayerController, platformHeights);
            
            foreach (Rectangle x in GameCanvas.Children.OfType<Rectangle>()) {
                if ((string) x.Tag != "enemy") continue;
                EnemyController enemy = new EnemyController(x);
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
            PlayerController.OnTick(Debug);
            PlatformHandler.OnTick(Debug);

            // enemy handling
            for (int i = 0; i <= Enemies.Count - 1; i++) {
                EnemyController enemy = Enemies[i];
                Debug.Content = "Dead";
                if (enemy.Dead) {
                    Enemies.Remove(enemy);
                    PlayerInformation.Score += 500;
                    i--;
                    continue;
                }
                
                enemy.OnTick(PlayerController, GameCanvas);
                
                foreach (Rectangle x in GameCanvas.Children.OfType<Rectangle>()) {
                    if ((string) x.Tag != "walkable") continue;

                    Rect floorHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // damage player
                    if (PlayerController.Hitbox.IntersectsWith(enemy.Hitbox)) PlayerController.TakeDamage(PlayerInformation);
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
            PlayerController.OnKeyDown(e);
        }
        
        /// <summary>
        /// fires when key is let loose
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">KeyEventArgs</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            PlayerController.OnKeyUp(e);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void Die(object sender, RoutedEventArgs e)
        {
            if (PlayerInformation.Multiplayer) {
                TwoPlayerDeathScreen death = new TwoPlayerDeathScreen(PlayerInformation);
                death.Visibility = Visibility.Visible;
                this.Close();
            } else {
                OnePlayerDeathScreen onePlayerDeathScreen = new OnePlayerDeathScreen(PlayerInformation);
                onePlayerDeathScreen.Visibility = Visibility.Visible;
                this.Close();   
            }
        }
    }
}