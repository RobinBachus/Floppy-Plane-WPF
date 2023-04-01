using System;
using System.Reflection.Metadata;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF
{
    public class Player
    {
        private Canvas Frame { get; }

        public Rectangle Sprite { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Speed { get; private set; }
        public bool HitFloor { get; private set; }
        public bool ShowHitboxes { get; set; }

        private const int START_POSITION = 200;
 
        public Player(Canvas canvas)
        {
            Frame = canvas;

            VisualBrush visualBrush = new();

            // TODO: Get relative paths working for all pc's
            string path = "Resources\\playerNoBG.png";

            // TODO: Try to animate player
            Image image = new()
            {
                Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute))
            };

            visualBrush.Visual = image;

            Sprite = new()
            {
                Name = "Player",
                Height = 0,
                Width = 125,
                Fill = visualBrush,
            };

            ShowHitboxes = false;

            X = 20;
            Y = START_POSITION;

            Speed = 1;

            image.Loaded += (sender, e) =>
            {
                double ratio = image.ActualHeight / image.ActualWidth;
                Sprite.Height = Sprite.Width * ratio;
            };

            Draw();
        }

        public void Draw()
        {
            if (!Frame.Children.Contains(Sprite)) Frame.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
            if (ShowHitboxes) Sprite.Stroke = Brushes.Green;
            else Sprite.Stroke = null;
        }

        public void MovePlayer()
        {
            if (Y+Sprite.Height < Frame.ActualHeight - 25) 
            {
                Y += Speed;
                Draw();
                if (Speed < 15) Speed++;

                Sprite.RenderTransform = new RotateTransform(Speed * .5);
            }
            else
            {
                HitFloor = true;
            }
        }

        public void Jump()
        {
            if (Y + Sprite.Height > Sprite.Height*2)
            {
                if (Speed >= 0) Speed = -10;
                else Speed = -15;
            }
        }

        public void SetToStartingPosition()
        {
            Y = START_POSITION;
            HitFloor = false;
            Sprite.RenderTransform = new RotateTransform(0);
            Draw();
        }
    }
}
