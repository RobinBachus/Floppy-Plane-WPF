using Floppy_Plane_WPF.GameObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Floppy_Plane_WPF.Controllers
{
    internal class PlayerGraphicsController
    {
        // TODO: Move player graphics logic to PlayerGraphicsController
        private readonly Player _player;    
        private List<Skin> Skins { get; } = new List<Skin>();
        private int CurrentSkinIndex { get; set; } = 0;

        public Skin CurrentSkin { get => Skins[CurrentSkinIndex]; }


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
        }
    }
}
