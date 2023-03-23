using System.Windows.Controls;
using System.Windows.Media;
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

        public Player(Canvas canvas)
        {
            frame = canvas;
            Rectangle sprite = new()
            {
                Name = "Player",
                Height = 55,
                Width = 100,
                Fill = Brushes.DarkViolet,
                Stroke = Brushes.LightSteelBlue,
            };
            Sprite = sprite;
            X = 20;
            Y = 150;

            Speed = 1;

            canvas.Children.Add(sprite);
            Canvas.SetLeft(sprite, X);
            Canvas.SetTop(sprite, Y);
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
    }
}
