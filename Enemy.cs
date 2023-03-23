using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF
{
    internal class Enemy
    {
        private readonly Canvas frame;
        public Rectangle Sprite { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public const int BaseSpeed = 1;
        public const int BaseHeight = 40;
        public const int BaseWidth = 70;

        public Enemy(Canvas canvas, int id, int yPosition)
        {

            frame = canvas;
            Sprite = new()
            {
                Name = $"Enemy{id}",
                Height = BaseHeight,
                Width = BaseWidth,
                Fill = Brushes.Red,
                Stroke = Brushes.DarkRed,
            };
            
            X = (int)(frame.ActualWidth);
            Y = yPosition;

            frame.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        public void Move()
        {
            Canvas.SetLeft(Sprite, X -= 10);
        }
    }
}
