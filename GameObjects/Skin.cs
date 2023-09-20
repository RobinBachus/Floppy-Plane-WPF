﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Floppy_Plane_WPF.GameObjects
{
    internal class Skin
    {
        public string Name { get; } 
        public Image IdleSprite {  get; }
        public Image JumpSprite { get; }

        public Skin(string sourceDir)
        {
            sourceDir = System.IO.Path.GetFullPath(sourceDir).Replace("\\", "/");
            Name = sourceDir[(sourceDir.LastIndexOf("/")+1)..];

            BitmapImage idleSource = new (new Uri($"{sourceDir}/player.png", UriKind.Absolute));
            IdleSprite = new() { Source = idleSource };

            BitmapImage jumpSource = new(new Uri($"{sourceDir}/player_AB.png", UriKind.Absolute));
            JumpSprite = new() { Source = jumpSource };
        }

        //private void SetSpriteRatio()
        //{
        //    VisualBrush _sprite = (VisualBrush)Sprite.Fill;
        //    Image _image = (Image)_sprite.Visual;
        //    double ratio = _image.ActualHeight / _image.ActualWidth;
        //    Sprite.Height = Sprite.Width * ratio;
        //}
    }
}
