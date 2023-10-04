using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Floppy_Plane_WPF.GameObjects
{
    internal class Skin
    {
        public string Name { get; } 
        public VisualBrush IdleBrush { get; }
        public VisualBrush JumpBrush { get; }

        public Skin(string sourceDir)
        {
            sourceDir = System.IO.Path.GetFullPath(sourceDir).Replace("\\", "/");
            Name = sourceDir[(sourceDir.LastIndexOf("/", StringComparison.Ordinal)+1)..];

            BitmapImage idleSource = new (new Uri($"{sourceDir}/player.png", UriKind.Absolute));
            IdleBrush = new VisualBrush { Visual = new Image() { Source = idleSource } };

            BitmapImage jumpSource = new(new Uri($"{sourceDir}/player_AB.png", UriKind.Absolute));
            JumpBrush = new VisualBrush { Visual = new Image() { Source = jumpSource } };
        }
    }
}
