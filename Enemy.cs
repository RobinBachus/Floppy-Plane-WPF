using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF
{
    internal class Enemy
    {
        private readonly Canvas frame;
        public Rectangle Sprite { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Level { get; private set; }

        public const int BaseSpeed = 10;
        public const int BaseHeight = 40;
        public const int BaseWidth = 70;

        public Enemy(Canvas canvas, int id, int yPosition, int level)
        {
            VisualBrush visualBrush = new();

            // TODO: Get relative paths working for all pc's
            string path = "Resources\\missile2.png";

            Image image = new()
            {
                Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute))
            };

            visualBrush.Visual = image;

            frame = canvas;
            Sprite = new()
            {
                Name = $"Enemy{id}",
                Height = BaseHeight,
                Width = BaseWidth,
                Fill = visualBrush,
            };
            
            X = (int)(frame.ActualWidth);
            Y = yPosition;

            image.Loaded += (sender, e) =>
            {
                double ratio = image.ActualHeight / image.ActualWidth;
                Sprite.Height = Sprite.Width * ratio;
            };

            Level = level;

            frame.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        public void Move()
        {
            Canvas.SetLeft(Sprite, X -= BaseSpeed + (Level*3));
        }
    }
}
