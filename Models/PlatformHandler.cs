using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tron_Mario.Models
{
    public class PlatformHandler
    {
        // a max of 60 between each platform height
        private readonly List<double> PlatformHeightsLeft = new List<double>();
        private readonly List<double> PlatformHeightsRight;
        private readonly Canvas GameCanvas;
        private readonly PlayerController PlayerController;
        private bool HasLeftCoordinates, HasRightCoordinates;
        public PlatformHandler(Canvas gameCanvas, PlayerController playerController, List<double> platformHeights)
        {
            GameCanvas = gameCanvas;
            PlayerController = playerController;
            PlatformHeightsRight = platformHeights;
            
            ImageBrush platformSkin = new ImageBrush {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Platform1.png"))};
            foreach (Rectangle x in GameCanvas.Children.OfType<Rectangle>()) {
                if (x.Name == "LarryTheGhostBox" || x.Name == "Floor") continue;
                if ((string) x.Tag != "walkable") continue;
                x.Fill = platformSkin;
            }
        }

        public void OnTick()
        {
            // platform logic
            foreach (Rectangle x in GameCanvas.Children.OfType<Rectangle>()) {
                if ((string) x.Tag != "walkable") continue;

                Rect floorHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if (PlayerController.Hitbox.IntersectsWith(floorHitbox)) {
                    PlayerController.HandleLanding(floorHitbox);
                    break;
                } else PlayerController.Fall();

                HasLeftCoordinates = PlatformHeightsLeft.Count > 0;
                HasRightCoordinates = PlatformHeightsRight.Count > 0;

                // larry the ghost box helps us get rid of a wacky glitch with the jumping logic
                if (x.Name == "LarryTheGhostBox") continue;

                double windowWith = Application.Current.MainWindow.Width;

                if (floorHitbox.Left > windowWith) { // player going left
                    if (!HasLeftCoordinates) PlayerController.MoveFree(false); // stop the camera
                    else {
                        Canvas.SetLeft(x, -floorHitbox.Width);
                        Canvas.SetTop(x, PlatformHeightsLeft.Last());
                        PlatformHeightsRight.Insert(0, floorHitbox.Top);
                        PlatformHeightsLeft.RemoveAt(PlatformHeightsLeft.Count - 1);
                    }
                } else if (floorHitbox.Left + floorHitbox.Width < 0) { // player going right
                    if (!HasRightCoordinates) PlayerController.MoveFree(true); // stop the camera
                    else {
                        Canvas.SetLeft(x, windowWith);
                        Canvas.SetTop(x,PlatformHeightsRight.First());
                        PlatformHeightsLeft.Add(floorHitbox.Top);
                        PlatformHeightsRight.RemoveAt(0);
                    }
                }  
            }
        }
    }
}