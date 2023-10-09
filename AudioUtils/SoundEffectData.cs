using System.Collections.Generic;

namespace Floppy_Plane_WPF.AudioUtils
{
    public enum SoundEffect
    {
        PlayerEngine,
        Explosion,
        Bgm
    }

    public class SoundEffectData
    {
        public static Dictionary<SoundEffect, SoundEffectData> SoundEffectDataList { get; } = new()
        {
            { SoundEffect.PlayerEngine, new SoundEffectData(SoundEffect.PlayerEngine, @"Resources\PlayerSkins\Default\engine.wav", true)},
            { SoundEffect.Explosion, new SoundEffectData(SoundEffect.Explosion, @"Resources\Sounds\explosion.wav", false) },
            { SoundEffect.Bgm, new SoundEffectData(SoundEffect.Bgm, @"Resources\Sounds\bgm.wav", true) }
        };

        public SoundEffect Effect { get; }
        public string FilePath { get; }
        public bool IsLooped { get; }

        public SoundEffectData(SoundEffect effect, string filePath, bool isLooped)
        {
            Effect = effect;
            FilePath = filePath;
            IsLooped = isLooped;
        }
    }
}
