using System;
using System.Windows.Media;

namespace Floppy_Plane_WPF.Controllers
{
    // TODO: Test sound controller
    internal class SoundController
    {
        private readonly MediaPlayer _mediaPlayer = new();

        public SoundController(string filePath)
        {
            SetSoundFile(filePath);
        }

        public void SetPitch(int pitch)
        {
            _mediaPlayer.SpeedRatio = pitch;
        }

        public void SetSoundFile(string filePath)
        {
            _mediaPlayer.Open(new Uri(filePath));
        }

        public void Play() { _mediaPlayer.Play(); }

        public void Stop() { _mediaPlayer.Stop(); }

        public void Pause() { _mediaPlayer.Pause(); } 
    }
}
