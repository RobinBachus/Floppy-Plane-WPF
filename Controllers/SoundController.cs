using System;
using System.Windows.Media;
using NAudio;
using NAudio.Wave;

namespace Floppy_Plane_WPF.Controllers
{
    // TODO: Test sound controller
    internal class SoundController
    {
        public bool Repeat { get; set; } = true;

        private readonly MediaPlayer _mediaPlayer = new();
        private readonly IWavePlayer _waveOutDevice = new WaveOut();

        public SoundController(Uri fileUri)
        {
            SetSoundFile(fileUri);
            _mediaPlayer.MediaEnded += Media_Ended;


            // Create a reader for your audio file (e.g., WAV, MP3)
            AudioFileReader audioFile = new(fileUri.LocalPath);

            // Initialize the audio player with the audio file
            _waveOutDevice.Init(audioFile);

        }

        public SoundController(string filePath)
            : this(new Uri(filePath)) { }

        public void SetSoundFile(Uri fileUri)
        {
            _mediaPlayer.Open(fileUri);
        }

        public void SetSoundFile(string filePath) => SetSoundFile(new Uri(filePath));

        public void SetPitch(double pitch)
        {
            _mediaPlayer.SpeedRatio = pitch;
        }

        public void Play()
        {
            _waveOutDevice.Play();
        }

        public void Stop()
        { _waveOutDevice.Stop(); }

        public void Pause()
        { _mediaPlayer.Pause(); }

        private void Media_Ended(object? sender, EventArgs e)
        {
            if (!Repeat) return;
            _mediaPlayer.Position = TimeSpan.Zero;
            Play();
        }
    }
}