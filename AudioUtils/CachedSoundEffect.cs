using NAudio.Wave;
using System;

namespace Floppy_Plane_WPF.AudioUtils
{
	public class CachedSoundEffect : IDisposable
	{
		public WaveStream Stream { get; }

		private readonly IWavePlayer _waveOutDevice;
		private bool _disposed;

		public CachedSoundEffect(WaveStream stream)
		{
			Stream = stream;
			_waveOutDevice = _waveOutDevice = new WaveOut();
			_waveOutDevice.Init(Stream);
		}

		public void Play()
		{
			// Play the cached audio
			_waveOutDevice.Play();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			
			_waveOutDevice.Dispose();
			Stream.Dispose();
			_disposed = true;
		}

		~CachedSoundEffect()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
