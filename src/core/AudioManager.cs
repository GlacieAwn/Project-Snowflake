using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Project_Snowflake
{
    public static class AudioManager
    {
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, SoundEffect> musicStreams = new Dictionary<string, SoundEffect>();

        private static SoundEffectInstance currentMusicInstance;

        public static void LoadSoundEffect(string key, SoundEffect soundEffect)
        {
            if (!soundEffects.ContainsKey(key))
            {
                soundEffects.Add(key, soundEffect);
            }
        }

        public static void PlaySoundEffect(string key)
        {
            if (soundEffects.ContainsKey(key))
            {
                soundEffects[key].Play();
            }
        }

        public static void LoadMusic(string key, SoundEffect music)
        {
            if (!musicStreams.ContainsKey(key))
            {
                musicStreams.Add(key, music);
            }
        }

        public static void PlayMusic(string key, bool loop)
        {
            if (!musicStreams.ContainsKey(key))
                return;

            StopMusic();

            currentMusicInstance = musicStreams[key].CreateInstance();
            currentMusicInstance.IsLooped = loop;
            currentMusicInstance.Play();
        }

        public static void StopMusic()
        {
            if (currentMusicInstance == null)
                return;

            currentMusicInstance.Stop();
            currentMusicInstance.Dispose();
            currentMusicInstance = null;
        }

        public static void PauseMusic()
        {
            if (currentMusicInstance != null && currentMusicInstance.State == SoundState.Playing)
            {
                currentMusicInstance.Pause();
            }
        }

        public static void ResumeMusic()
        {
            if (currentMusicInstance != null && currentMusicInstance.State == SoundState.Paused)
            {
                currentMusicInstance.Resume();
            }
        }
    }
}