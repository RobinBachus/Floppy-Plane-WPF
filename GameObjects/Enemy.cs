using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF.GameObjects
{
    internal class Enemy
    {
        private Canvas Frame { get; }
        public Rectangle Sprite { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Level { get; private set; }

        public const int BaseSpeed = 10;
        public const int BaseHeight = 40;
        public const int BaseWidth = 70;

        /** <summary> Value that the level is multiplied with to get the speed of the enemy </summary> */
        private int SpeedIncreaseValue { get; }

        public Enemy(Canvas canvas, int id, int yPosition, int level, int speedIncreaseValue, bool showHitbox)
        {
            VisualBrush visualBrush = new();

            string path = @"Resources\missile.png";

            Image image = new()
            {
                Source = new BitmapImage(new Uri(path, UriKind.Relative))
            };

            visualBrush.Visual = image;

            Frame = canvas;
            Sprite = new()
            {
                Name = $"Enemy{id}",
                Height = BaseHeight,
                Width = BaseWidth,
                Fill = visualBrush,
            };

            if (showHitbox) Sprite.Stroke = Brushes.Red;

            X = (int)Frame.ActualWidth;
            Y = yPosition;

            image.Loaded += (sender, e) =>
            {
                double ratio = image.ActualHeight / image.ActualWidth;
                Sprite.Height = Sprite.Width * ratio;
            };

            Level = level;

            SpeedIncreaseValue = speedIncreaseValue;

            Frame.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        public void Move() => Canvas.SetLeft(Sprite, X -= BaseSpeed + Level * SpeedIncreaseValue);
    }
}
