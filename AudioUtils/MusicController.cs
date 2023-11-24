using System;
using NAudio.Wave;

namespace Floppy_Plane_WPF.AudioUtils
{
    public class MusicController: IDisposable
    {
        private readonly IWavePlayer _waveOutDevice;
        private readonly LoopStream _stream;
        private readonly CachedSoundEffect _musicCache;
        private bool _disposed;

        public MusicController()
        {
            // Initialize the audio player (WaveOut or other options)
            _waveOutDevice = new WaveOut();

            SoundEffectData bgmData = SoundEffectData.SoundEffectDataList[SoundEffect.Bgm];

            _stream = new LoopStream(new AudioFileReader(bgmData.FilePath));
            _musicCache = new CachedSoundEffect(_stream);

            _waveOutDevice.Init(_stream);
            _waveOutDevice.Volume = .5f;
        }

        public void Play()
        {
            _stream.EnableLooping = true;
            _musicCache.Play();
        }

        public void Stop()
        {
            _stream.EnableLooping = false;
            _waveOutDevice.Stop();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _waveOutDevice.Dispose();
            }

            _musicCache.Dispose();
            _stream.Dispose();
            _disposed = true;
        }

        ~MusicController()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
