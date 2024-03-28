using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Floppy_Plane_WPF.GUI;

namespace Floppy_Plane_WPF.GameObjects
{
    internal class Enemy
    {
        private Canvas Frame { get; }
        public Rectangle Sprite { get; }

        public int X { get; private set; }
        public int Y { get; }
        public int Level { get; }

        private static byte _id = 0;
        private static byte NextId => _id = (byte)((_id + 1) % 256);

        public const int BaseSpeed = 10;
        public const int BaseHeight = 40;
        public const int BaseWidth = 70;

        /** <summary> Value that the level is multiplied with to get the speed of the enemy </summary> */
        private int SpeedIncreaseValue { get; }

        private const string TEXTURE_PATH = @"Resources\missile.png";

        public Enemy(Canvas canvas, int yPosition, int level, int speedIncreaseValue)
        {
            VisualBrush visualBrush = new();


            Image image = new()
            {
                Source = new BitmapImage(new Uri(TEXTURE_PATH, UriKind.Relative))
            };

            visualBrush.Visual = image;

            Frame = canvas;
            Sprite = new Rectangle
            {
                Name = $"Enemy{NextId}",
                Height = BaseHeight,
                Width = BaseWidth,
                Fill = visualBrush,
            };

            if (Settings.ShowHitBoxes) Sprite.Stroke = Brushes.Red;

            X = (int)Frame.ActualWidth;
            Y = yPosition;

            image.Loaded += delegate
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
