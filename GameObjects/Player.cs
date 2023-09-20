using Floppy_Plane_WPF.Controllers;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF
{
    public class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public double Speed { get; private set; }
        public bool HitFloor { get; private set; }

        public Rectangle Sprite { get => GraphicsController.Sprite; }
        public bool IsJumping { get => Speed < 0; }
        public bool ShowHitBoxes 
        {
            get => GraphicsController.ShowHitBoxes; 
            set => GraphicsController.ShowHitBoxes = value;
        }

        private Canvas Frame { get; }
        private PlayerGraphicsController GraphicsController { get; }

        private RotateTransform Rotation { get => GraphicsController.Rotation; }

        private const int START_HEIGHT = 200;
 
        public Player(Canvas canvas)
        {
            Frame = canvas;

            X = 20;
            Y = START_HEIGHT;
            Speed = 1;

            GraphicsController = new PlayerGraphicsController(this);
            ShowHitBoxes = false;
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
                GraphicsController.AdjustSprite();
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
            Y = START_HEIGHT;
            HitFloor = false;
            Rotation.Angle = 0;
            Draw();
        }
    }
}
