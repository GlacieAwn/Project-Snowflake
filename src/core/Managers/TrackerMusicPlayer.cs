using System;
using System.IO;
using System.Threading;
using LibOpenMPT.NET;
using Microsoft.Xna.Framework.Audio;

namespace Project_Snowflake
{
    /// <summary>
    /// Streams tracker modules (MOD, S3M, IT, XM, ...) decoded by libopenmpt
    /// into a DynamicSoundEffectInstance from a background thread.
    /// </summary>
    public sealed unsafe class TrackerMusicPlayer : IDisposable
    {
        private const int SampleRate = 48000;
        private const int Channels = 2;
        private const int BytesPerFrame = Channels * sizeof(short);
        private const int ChunkMs = 100;
        private const int TargetQueuedBuffers = 4;
        private const int FramesPerChunk = SampleRate * ChunkMs / 1000;

        private readonly object _lock = new object();
        private readonly byte[] _chunk = new byte[FramesPerChunk * BytesPerFrame];

        private Module* _module;
        private DynamicSoundEffectInstance _instance;
        private Thread _thread;
        private volatile bool _running;
        private volatile bool _paused;
        private float _volume = 1f;

        public bool IsPlaying => _instance != null && _instance.State == SoundState.Playing;

        public float Volume
        {
            get { lock (_lock) return _instance?.Volume ?? _volume; }
            set
            {
                lock (_lock)
                {
                    _volume = Math.Clamp(value, 0f, 1f);
                    if (_instance != null)
                        _instance.Volume = _volume;
                }
            }
        }

        public void Play(string filePath, bool loop)
        {
            Stop();

            byte[] data = File.ReadAllBytes(filePath);
            Module* module;
            fixed (byte* p = data)
            {
                // libopenmpt copies the data during load, so unpinning afterwards is safe.
                module = NativeMethods.module_create_from_memory2(p, (UIntPtr)data.Length, null, null, null, null, null, null, null);
            }
            if (module == null)
                throw new InvalidDataException($"libopenmpt could not load '{filePath}'.");

            lock (_lock)
            {
                _module = module;
                _paused = false;
                // -1 lets libopenmpt loop seamlessly inside one read stream; seeking to 0 would reset channel state.
                NativeMethods.module_set_repeat_count(_module, loop ? -1 : 0);
                _instance = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Stereo) { Volume = _volume };

                // Prime the queue so playback starts without an underrun.
                FillBuffers();
                _instance.Play();
            }

            _running = true;
            _thread = new Thread(StreamLoop) { IsBackground = true, Name = "TrackerMusicPlayer" };
            _thread.Start();
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (_instance == null || _instance.State != SoundState.Playing)
                    return;
                _paused = true;
                _instance.Pause();
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (_instance == null || !_paused)
                    return;
                _paused = false;
                _instance.Resume();
            }
        }

        public void Stop()
        {
            _running = false;
            Thread thread = _thread;
            _thread = null;
            if (thread != null && thread != Thread.CurrentThread)
                thread.Join();

            lock (_lock)
            {
                if (_instance != null)
                {
                    _instance.Stop();
                    _instance.Dispose();
                    _instance = null;
                }
                if (_module != null)
                {
                    NativeMethods.module_destroy(_module);
                    _module = null;
                }
                _paused = false;
            }
        }

        public void Dispose() => Stop();

        private void StreamLoop()
        {
            while (_running)
            {
                bool finished;
                lock (_lock)
                {
                    finished = _module != null && !_paused && FillBuffers();
                }

                if (finished)
                {
                    // Non-looping module ended: let the queued audio drain, then stop.
                    while (_running && _instance != null && _instance.PendingBufferCount > 0)
                        Thread.Sleep(20);
                    lock (_lock)
                    {
                        if (_running && _instance != null && _instance.State == SoundState.Playing)
                            _instance.Stop();
                    }
                    return;
                }

                Thread.Sleep(ChunkMs / 4);
            }
        }

        // Must be called with _lock held. Returns true once a non-looping module has ended.
        private bool FillBuffers()
        {
            while (_instance.PendingBufferCount < TargetQueuedBuffers)
            {
                int frames = ReadFrames();
                if (frames <= 0)
                    return true;
                _instance.SubmitBuffer(_chunk, 0, frames * BytesPerFrame);
            }
            return false;
        }

        private int ReadFrames()
        {
            fixed (byte* p = _chunk)
            {
                return (int)NativeMethods.module_read_interleaved_stereo(_module, SampleRate, (UIntPtr)FramesPerChunk, (short*)p);
            }
        }
    }
}
