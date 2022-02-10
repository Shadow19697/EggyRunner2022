using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Scripts.Managers.Sounds
{
    public static class SoundManager
    {
        public static AudioMixer _audioMixer;

        public static List<AudioSource> _levelMusic;
        public static AudioSource _cinematicMusic;
        public static AudioSource _endingMusic;

        public static AudioSource _jumpSound;
        public static AudioSource _grabPerkSound;
        public static AudioSource _grabEggSound;
        public static AudioSource _gameOverSound;

        public static void InitVolume()
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        public static void MuteMusic(bool value)
        {
            if (value) _audioMixer.SetFloat("MusicVolume", 0.0001f);
            else _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
        }

        public static void MuteSoundEffects(bool value)
        {
            if (value) _audioMixer.SetFloat("SoundEffectsVolume", 0.0001f);
            else _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        #region Music
        public static void PlayLevelMusic()
        {
            _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Play();
        }

        public static void PauseLevelMusic()
        {
            _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Pause();
        }

        public static void PlayCinematicMusic()
        {
            _cinematicMusic.Play();
        }

        public static void StopCinematicMusic()
        {
            _cinematicMusic.Stop();
        }

        public static void PlayEndingMusic()
        {
            _endingMusic.Play();
        }

        public static void StopEndingMusic()
        {
            _endingMusic.Stop();
        }
        #endregion

        #region Sounds

        #endregion
    }
}