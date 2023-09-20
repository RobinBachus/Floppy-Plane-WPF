using Floppy_Plane_WPF.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF.Controllers
{
    internal class PlayerGraphicsController
    {
        public Rectangle Sprite { get; }
        public RotateTransform Rotation { get; } = new(0);

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

        // TODO: Move player graphics logic to PlayerGraphicsController
        private readonly Player _player;
        private List<Skin> Skins { get; } = new List<Skin>();
        private int CurrentSkinIndex { get; set; } = 0;
        private bool HasJumpSkin { get; set; } = false;

        private VisualBrush IdleBrush { get => new() { Visual = CurrentSkin.IdleImage }; }
        private VisualBrush JumpBrush { get => new() { Visual = CurrentSkin.JumpImage }; }
        private Skin CurrentSkin { get => Skins[CurrentSkinIndex]; }


        public PlayerGraphicsController(Player player)
        {
            _player = player;
            var skinPaths = Directory.GetDirectories(@"Resources\PlayerSkins\");

            int i = 0;
            foreach (var skinPath in skinPaths)
            {
                Skin _skin = new(skinPath);
                Skins.Add(_skin);

                if (_skin.Name.ToLower() == "default") CurrentSkinIndex = i;
                i++;
            }

            Sprite = new()
            {
                Name = "Player",
                Height = 0,
                Width = 125,
                Fill = IdleBrush,
                RenderTransform = Rotation
            };

            CurrentSkin.IdleImage.Loaded += (sender, e) => SetSpriteRatio();
        }

        public void AdjustSprite()
        {
            bool useJumpSkin;
            // Change the sprite when the plane starts to go up or down
            if (_player.IsJumping && !HasJumpSkin) useJumpSkin = true;
            else if (!_player.IsJumping && HasJumpSkin) useJumpSkin = false;
            // Don't change anything if the sprite doesn't change
            else return;

            Sprite.Fill = useJumpSkin ? JumpBrush : IdleBrush;
            HasJumpSkin = useJumpSkin;
        }

        public void NextSprite()
        {
            CurrentSkinIndex = (CurrentSkinIndex + 1) % Skins.Count; 
        }

        public void PrevSprite()
        {
            CurrentSkinIndex = (CurrentSkinIndex + Skins.Count - 1) % Skins.Count;
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
