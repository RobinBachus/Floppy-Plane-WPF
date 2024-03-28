using Floppy_Plane_WPF.Controllers;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Floppy_Plane_WPF.GUI;

namespace Floppy_Plane_WPF.GameObjects
{
	public class Player
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool HitFloor { get; private set; }
		public PlayerGraphicsController GraphicsController { get; }
		public Canvas Frame { get; }

		public static double GravityMultiplier { get; set; }

		public Rectangle Sprite => GraphicsController.Sprite;
		public bool IsJumping => Speed < 0;

		//public bool ShowHitBoxes 
		//{
		//	get => GraphicsController.ShowHitBoxes; 
		//	set => GraphicsController.ShowHitBoxes = value;
		//}

		private RotateTransform Rotation => GraphicsController.Rotation;

		private double Speed {  get; set; }

		private const int START_HEIGHT = 200;
		private const int START_LEFT_MARGIN = 20;

		public Player(Canvas canvas)
		{
			Frame = canvas;

			X = START_LEFT_MARGIN;
			Y = START_HEIGHT;
			Speed = 1;

			GraphicsController = new PlayerGraphicsController(this);
			GravityMultiplier = 1;
		}

		public void Draw()
		{
			if (!Frame.Children.Contains(Sprite)) Frame.Children.Add(Sprite);
			Canvas.SetLeft(Sprite, X);
			Canvas.SetTop(Sprite, Y);

			Sprite.Stroke = Settings.ShowHitBoxes ? Brushes.Green : null;

			Frame.Focus();
		}

		public void MovePlayer()
		{
			if (Y + Sprite.ActualHeight < Frame.ActualHeight - 25) 
			{
				Y += Convert.ToInt32(Math.Floor(Speed));
				Draw();
				if (Speed < 15) Speed += 1 * GravityMultiplier;

				Rotation.Angle = Speed * .75;
				GraphicsController.AdjustSprite();

				//double pitch = (Speed + 15) / 30 + 1;
				//GraphicsController.SetAudioPitch(2.0);
			}
			else HitFloor = true;
		}

		public void Jump()
		{
			if (Y + Sprite.ActualHeight > Sprite.ActualHeight / 2) 
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
