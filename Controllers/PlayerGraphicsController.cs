using Floppy_Plane_WPF.GameObjects;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Floppy_Plane_WPF.AudioUtils;
using Floppy_Plane_WPF.GUI;

namespace Floppy_Plane_WPF.Controllers
{
    public class PlayerGraphicsController
    {
        public Rectangle Sprite { get; }
        public RotateTransform Rotation { get; } = new(0);
        public string CurrentSkinName => CurrentSkin.Name;

        // Use Settings.ShowHitBoxes instead
        //private bool _showHitboxes;
        //public bool ShowHitBoxes
        //{
        //    get => _showHitboxes;
        //    set
        //    {
        //        Sprite.Stroke = value ? Brushes.Green : null;
        //        _showHitboxes = value;
        //    }
        //}

        private readonly Player _player;
        private readonly SoundEffectCache _soundEffectCache;
        private List<Skin> Skins { get; } = new();
        private int CurrentSkinIndex { get; set; }
        private bool HasJumpSkin { get; set; }

        private VisualBrush IdleBrush => CurrentSkin.IdleBrush;
        private VisualBrush JumpBrush => CurrentSkin.JumpBrush;
        private Skin CurrentSkin => Skins[CurrentSkinIndex];


        public PlayerGraphicsController(Player player)
        {
            _player = player;
            string[] skinPaths = Directory.GetDirectories(@"Resources\PlayerSkins\");

            int i = 0;
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

            _soundEffectCache = new SoundEffectCache();
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

        public void StartSound() => _soundEffectCache.PlayCachedSoundEffect(SoundEffect.PlayerEngine);

        public void PlayDeathSound() 
        {
            _soundEffectCache.StopSoundEffects();
            _soundEffectCache.PlayCachedSoundEffect(SoundEffect.Explosion);
        }

        private void SetCurrentSkinIndex(int spriteIndex)
        {
            CurrentSkinIndex = spriteIndex;
            Sprite.Fill = _player.IsJumping ? JumpBrush : IdleBrush;
            SetSpriteRatio();
        }

        private void SetSpriteRatio()
        {
            VisualBrush? sprite = (VisualBrush)Sprite.Fill;
            Image? image = (Image)sprite.Visual;
            double ratio = image.ActualHeight / image.ActualWidth;
            Sprite.Height = Sprite.Width * ratio;
        }
    }
}
