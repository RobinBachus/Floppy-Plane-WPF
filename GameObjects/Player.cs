using Floppy_Plane_WPF.Controllers;
using Floppy_Plane_WPF.GameObjects;
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
        public Rectangle Sprite { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public double Speed { get; private set; }
        public bool HitFloor { get; private set; }

        private bool _showHitboxes;
        public bool ShowHitBoxes
        {
            get => _showHitboxes; 
            set
            {
                Sprite.Stroke = value ? Brushes.Green : null;
                _showHitboxes = value;
            }
        }

        private Canvas Frame { get; }
        private List<VisualBrush> Sprites { get; } = new(2);
        private PlayerGraphicsController GraphicsController { get; }
        private int CurrentSprite { get; set; } = 0;
        private RotateTransform Rotation { get; } = new(0);

        private const int START_POSITION = 200;
 
        public Player(Canvas canvas)
        {
            Frame = canvas;

            Skin skin = new(@"Resources\PlayerSkins\Default");
            GraphicsController = new PlayerGraphicsController(this);

            Sprites.Add(new() { Visual = GraphicsController.CurrentSkin.IdleSprite });

            Sprite = new()
            {
                Name = "Player",
                Height = 0,
                Width = 125,
                Fill = Sprites[0],
                RenderTransform = Rotation,
            };

            ShowHitBoxes = false;

            X = 20;
            Y = START_POSITION;

            Speed = 1;

            GraphicsController.CurrentSkin.IdleSprite.Loaded += (sender, e) =>
            {
                SetSpriteRatio();
            };

            Sprites.Add(new() { Visual = GraphicsController.CurrentSkin.JumpSprite });
        }

        public void Draw()
        {
            if (!Frame.Children.Contains(Sprite)) Frame.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
            Frame.Focus();
        }

        public void MovePlayer()
        {
            if (Y+Sprite.Height < Frame.ActualHeight - 25) 
            {
                Y += Convert.ToInt32(Math.Floor(Speed));
                Draw();
                if (Speed < 15) Speed += 1;

                Rotation.Angle = Speed * .5;
                AdjustSprite();
            }
            else HitFloor = true;
        }

        public void Jump()
        {
            if (Y + Sprite.Height > Sprite.Height*2) 
                // Regular jump is -10 and double jump is -15
                Speed = (Speed >= 0) ? -10 : -15;
        }

        public void SetToStartPosition()
        {
            Y = START_POSITION;
            HitFloor = false;
            Rotation.Angle = 0;
            Draw();
        }

        private void AdjustSprite()
        {
            // Change the sprite when the plane starts to go up or down
            int spriteIndex = (Speed < 0 && CurrentSprite == 0) ? 1 : (Speed > 0 && CurrentSprite == 1) ? 0 : CurrentSprite;
            // Don't change anything if the sprite doesn't change
            if (spriteIndex == CurrentSprite) return;
            Sprite.Fill = Sprites[spriteIndex];
            CurrentSprite = spriteIndex;
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
