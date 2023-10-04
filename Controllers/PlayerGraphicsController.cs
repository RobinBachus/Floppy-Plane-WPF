using Floppy_Plane_WPF.GameObjects;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF.Controllers
{
    public class PlayerGraphicsController
    {
        public Rectangle Sprite { get; }
        public string CurrentSkinName => CurrentSkin.Name;
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
        private int CurrentSkinIndex { get; set; }
        private bool HasJumpSkin { get; set; }

        private VisualBrush IdleBrush => CurrentSkin.IdleBrush;
        private VisualBrush JumpBrush => CurrentSkin.JumpBrush;
        private Skin CurrentSkin => Skins[CurrentSkinIndex];


        public PlayerGraphicsController(Player player)
        {
            _player = player;
            string[] skinPaths = Directory.GetDirectories(@"Resources\PlayerSkins\");

            var i = 0;
            foreach (string skinPath in skinPaths)
            {
                Skin skin = new(skinPath);
                Skins.Add(skin);

                if (skin.Name.ToLower() == "default") CurrentSkinIndex = i;
                i++;
            }

            Sprite = new Rectangle
            {
                Name = "Player",
                Height = 0,
                Width = 125,
                Fill = IdleBrush,
                RenderTransform = Rotation
            };

            ((Image)CurrentSkin.IdleBrush.Visual).Loaded += delegate { SetSpriteRatio(); };
        }

        public void AdjustSprite()
        {
            bool useJumpSkin;
            switch (_player.IsJumping)
            {
                // Change the sprite when the plane starts to go up or down
                case true when !HasJumpSkin:
                    useJumpSkin = true;
                    break;
                case false when HasJumpSkin:
                    useJumpSkin = false;
                    break;
                // Don't change anything if the sprite doesn't change
                default:
                    return;
            }

            Sprite.Fill = useJumpSkin ? JumpBrush : IdleBrush;
            HasJumpSkin = useJumpSkin;
        }

        public void NextSprite() => SetCurrentSkinIndex((CurrentSkinIndex + 1) % Skins.Count);

        public void PrevSprite() => SetCurrentSkinIndex((CurrentSkinIndex + Skins.Count - 1) % Skins.Count);

        private void SetCurrentSkinIndex(int spriteIndex)
        {
            CurrentSkinIndex = spriteIndex;
            Sprite.Fill = _player.IsJumping ? JumpBrush : IdleBrush;
            SetSpriteRatio();
        }

        private void SetSpriteRatio()
        {
            var sprite = (VisualBrush)Sprite.Fill;
            var image = (Image)sprite.Visual;
            double ratio = image.ActualHeight / image.ActualWidth;
            Sprite.Height = Sprite.Width * ratio;
        }
    }
}
