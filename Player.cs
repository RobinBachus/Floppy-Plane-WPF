using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF
{
    public class Player
    {
        private readonly Canvas frame;
        public Rectangle Sprite { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Speed { get; private set; }
        public bool HitFloor { get; private set; }

        public Player(Canvas canvas)
        {
            frame = canvas;

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

            X = 20;
            Y = 150;

            Speed = 1;

            image.Loaded += (sender, e) =>
            {
                double ratio = image.ActualHeight / image.ActualWidth;
                Sprite.Height = Sprite.Width * ratio;
            };

            canvas.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        public void MovePlayer()
        {
            if (Y+Sprite.Height < frame.ActualHeight - 25) 
            {
                Y += Speed;
                Canvas.SetTop(Sprite, Y);
                if (Speed < 15) Speed++;
            }
            else
            {
                HitFloor = true;
                Y = (int)((frame.ActualHeight - 25) - Sprite.Height);
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
            Y = 150;
            HitFloor = false;
        }
    }
}
