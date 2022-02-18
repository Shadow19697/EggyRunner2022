using Scripts.Controllers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Managers.InGame
{
    public class SoundManager : MonoBehaviour
    {
        public AudioMixer _audioMixer;

        public List<AudioSource> _levelMusic;
        public AudioSource _cinematicMusic;
        public AudioSource _endingMusic;
        public AudioSource _powerupMusic;

        public AudioSource _gameOverSound;
        public AudioSource _grabEggSound;
        public AudioSource _grabPerkSound;
        public AudioSource _jumpSound;

        private void Start()
        {
            SettingsController.SetVolume(_audioMixer);
        }

        public void InitVolume()
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        public void MuteMusic(bool value)
        {
            if (value) _audioMixer.SetFloat("MusicVolume", 0.0001f);
            else _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
        }

        public void MuteSoundEffects(bool value)
        {
            if (value) _audioMixer.SetFloat("SoundEffectsVolume", 0.0001f);
            else _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        #region Music
        public void PlayLevelMusic()
        {
            _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Play();
        }

        public void PauseLevelMusic()
        {
            _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Pause();
        }

        public void PlayCinematicMusic()
        {
            _cinematicMusic.Play();
        }

        public void StopCinematicMusic()
        {
            _cinematicMusic.Stop();
        }

        public void PlayEndingMusic()
        {
            _endingMusic.Play();
        }

        public void StopEndingMusic()
        {
            _endingMusic.Stop();
        }

        public void PlayPowerupMusic()
        {
            _powerupMusic.Play();
        }

        public void StopPowerupMusic()
        {
            _powerupMusic.Stop();
        }
        #endregion

        #region Sounds
        public void PlayGameOverSound()
        {
            _gameOverSound.Play();
        }

        public void PlayGrabEggSound()
        {
            _grabEggSound.Play();
        }

        public void PlayGrabPerkSound()
        {
            _grabPerkSound.Play();
        }

        public void PlayJumpSound()
        {
            _jumpSound.Play();
        }
        #endregion
    }
}