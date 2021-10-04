using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tron_Mario.Models;

namespace Tron_Mario
{
    public partial class Level1 : Window
    {
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private PlayerController Controller { get; set; }

        public Level1()
        {
            InitializeComponent();
            
            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += GameEngine;
            GameTimer.Start();
            
            Controller = new PlayerController(GameCanvas, Player);

            GameCanvas.Focus();
        }

        // engine; is called every 16 milliseconds
        private void GameEngine(object sender, EventArgs e)
        {
            Controller.Move();
        }

        // movement
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Controller.OnKeyDown(e);
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Controller.OnKeyUp(e);
        }
    }
}