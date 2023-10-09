using NAudio.Wave;
using System;

namespace Floppy_Plane_WPF.AudioUtils
{
    public class CachedSoundEffect : IDisposable
    {
        public WaveStream Stream { get; }

        private bool _disposed;

        public CachedSoundEffect(WaveStream stream)
        {
            Stream = stream;
        }

        public void Play(IWavePlayer waveOutDevice)
        {
            // Play the cached audio
            waveOutDevice.Init(Stream);
            waveOutDevice.Play();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            
            Stream.Dispose();
            _disposed = true;

            
        }
    }
}
