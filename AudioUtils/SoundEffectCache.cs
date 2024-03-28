using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace Floppy_Plane_WPF.AudioUtils
{
	public class SoundEffectCache : IDisposable
	{
		private readonly IWavePlayer _waveOutDevice;
		private readonly Dictionary<SoundEffect, CachedSoundEffect> _cache = new();
		private bool _disposed;

		public SoundEffectCache()
		{
			// Initialize the audio player (WaveOut or other options)
			_waveOutDevice = new WaveOut();

			foreach (KeyValuePair<SoundEffect, SoundEffectData> soundData in SoundEffectData.SoundEffectDataList) CacheSoundEffect(soundData.Value);
		}

		public void CacheSoundEffect(SoundEffectData data)
		{
			if (_cache.ContainsKey(data.Effect)) return;
			WaveStream stream;
			AudioFileReader audioFile = new AudioFileReader(data.FilePath);
			if (data.IsLooped) stream = new LoopStream(audioFile);
			else stream = audioFile;
			_cache[data.Effect] = new CachedSoundEffect(stream);
		}

		public void PlayCachedSoundEffect(SoundEffect soundEffect)
		{
			if (!_cache.TryGetValue(soundEffect, out CachedSoundEffect? cachedSoundEffect)) return;
			cachedSoundEffect.Play();
			ToggleLooping(cachedSoundEffect, true);
		}

		public void StopSoundEffects()
		{
			foreach (CachedSoundEffect soundEffect in _cache.Values)
				ToggleLooping(soundEffect, false);

			_waveOutDevice.Stop();
			
		}

		private static void ToggleLooping(CachedSoundEffect soundEffect, bool looping)
		{
			if (soundEffect.Stream is LoopStream ls) ls.EnableLooping = looping;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing)
			{
				// TODO: dispose managed state (managed objects)
			}

			// TODO: free unmanaged resources (unmanaged objects) and override finalizer

			_waveOutDevice.Stop();
			_waveOutDevice.Dispose();

			foreach (CachedSoundEffect cachedSoundEffect in _cache.Values)
			{
				cachedSoundEffect.Dispose();
			}

			// TODO: set large fields to null
			_disposed = true;
		}

		// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~SoundEffectCache()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

	}
}