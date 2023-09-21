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
        public PlayerGraphicsController GraphicsController { get; }

        public Rectangle Sprite { get => GraphicsController.Sprite; }
        public bool IsJumping { get => Speed < 0; }
        public bool ShowHitBoxes 
        {
            get => GraphicsController.ShowHitBoxes; 
            set => GraphicsController.ShowHitBoxes = value;
        }

        private Canvas Frame { get; }
        private RotateTransform Rotation { get => GraphicsController.Rotation; }

        private const int START_HEIGHT = 200;
        private const int START_LEFT_MARGIN = 20;

        public Player(Canvas canvas)
        {
            Frame = canvas;

            X = START_LEFT_MARGIN;
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

        public void MoveTo(int x, int y, int? stepX = null, int? stepY = null)
        {
            _dest_x = x;
            _dest_y = y;

            _step_x = stepX ?? (_dest_x - X) / 5;
            _step_y = stepY ?? (_dest_y - Y) / 5;
        }

        public void MoveToX(int x) => MoveTo(x, Y);
        public void MoveToY(int y) => MoveTo(X, y);

        private int? _dest_x;
        private int? _dest_y;

        private int? _step_x;
        private int? _step_y;

        private void _UpdateMove()
        {
            if (_dest_x != null && X - _step_x >= _step_x)
                X += _step_x ?? 0;
            else if (_dest_x != null && X - _step_x < _step_x)
                _dest_x = null;

            if (_dest_y != null && Y -  _step_y >= _step_y)
                Y += _step_y ?? 0;
            else if (_dest_y != null && Y - _step_y < _step_y)
                _dest_y = null;
        }

        public void Jump()
        {
            if (Y + Sprite.Height > Sprite.Height*2) 
                // Regular jump is -10 and double jump is -15
                Speed = (Speed >= 0) ? -10 : -15;
        }

        public void SetToStartPosition()
        {
            X = START_LEFT_MARGIN;
            Y = START_HEIGHT;
            HitFloor = false;
            Rotation.Angle = 0;
            Draw();
        }
    }
}
