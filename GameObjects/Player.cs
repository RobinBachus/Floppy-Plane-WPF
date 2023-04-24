using System;
using System.Collections.Generic;
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

        private List<VisualBrush> Sprites { get; } = new(2);
        private const int START_POSITION = 200;
 
        public Player(Canvas canvas)
        {
            Frame = canvas; 

            Image idleSprite = new()
            {
                Source = new BitmapImage(new Uri(@"Resources\player.png", UriKind.Relative))
            };

            Sprites.Add(new() { Visual = idleSprite });

            Sprite = new()
            {
                Name = "Player",
                Height = 0,
                Width = 125,
                Fill = Sprites[0],
            };

            ShowHitboxes = false;

            X = 20;
            Y = START_POSITION;

            Speed = 1;

            idleSprite.Loaded += (sender, e) =>
            {
                SetSpriteRatio();
            };

            Image jumpSprite = new()
            {
                Source = new BitmapImage(new Uri(@"Resources\player_AB.png", UriKind.Relative))
            };

            Sprites.Add(new() { Visual = jumpSprite });
        }

        public void Draw()
        {
            if (!Frame.Children.Contains(Sprite)) Frame.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
            if (ShowHitboxes) Sprite.Stroke = Brushes.Green;
            else Sprite.Stroke = null;

            Frame.Focus();
        }

        public void MovePlayer()
        {
            if (Y+Sprite.Height < Frame.ActualHeight - 25) 
            {
                Y += Speed;
                Draw();
                if (Speed < 15) Speed++;

                Sprite.RenderTransform = new RotateTransform(Speed * .5);
                AdjustSprite();
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

        private void AdjustSprite()
        {
            if (Speed < 0 && Sprite.Fill.Equals(Sprites[0]))
            {
                Sprite.Fill = Sprites[1];
            }
            else if (Speed > 0 && Sprite.Fill.Equals(Sprites[1])) 
            { 
                Sprite.Fill = Sprites[0];
            }
        }

        private void SetSpriteRatio()
        {
            VisualBrush _sprite = (VisualBrush)Sprite.Fill;
            Image _image = (Image)_sprite.Visual;
            double ratio = _image.ActualHeight / _image.ActualWidth;
            Sprite.Height = Sprite.Width * ratio;
        }
    }
}
