using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tron_Mario.Models;

namespace Tron_Mario
{
    public partial class Level1 : Window
    {
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private PlayerController Controller;

        public Level1()
        {
            InitializeComponent();
            
            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += GameEngine;
            GameTimer.Start();
            
            // call inside level initializer to create the player controller
            Controller = new PlayerController(GameCanvas, Player);
            GameCanvas.Focus();
        }
        
        /// <summary>
        /// engine is called every 16 milliseconds
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">eventargs</param>
        private void GameEngine(object sender, EventArgs e)
        {
            Controller.OnTick(Debug);

            foreach (Rectangle x in GameCanvas.Children.OfType<Rectangle>()) {
                if ((string) x.Tag != "walkable") continue;
                Rect floorHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if (Controller.PlayerHitbox.IntersectsWith(floorHitbox)) Controller.HandleLanding(x);
                else Controller.Fall();
            }
        }

        // movement
        /// <summary>
        /// fires when key is pressed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">keyeventargs</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Controller.OnKeyDown(e);
        }
        /// <summary>
        /// fires when key is let loose
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">keyeventargs</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Controller.OnKeyUp(e);
        }
    }
}