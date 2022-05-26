using Scripts.Controllers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Managers.InGame
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private List<AudioSource> _levelMusic;
        [SerializeField] private List<AudioSource> _cinematicMusic;
        [SerializeField] private AudioSource _endingMusic;
        [SerializeField] private AudioSource _powerupMusic;

        private bool _isMusicPlaying = true;
        private bool _isPowerUpPlaying = false;

        private static SoundManager _instance;
        public static SoundManager Instance { get { if (_instance == null) _instance = FindObjectOfType<SoundManager>(); return _instance; } }

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
            if (value) _audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.0001f)*20);
            else _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
        }

        public void MuteSoundEffects(bool value)
        {
            if (value) _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(0.0001f) * 20);
            else _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        #region Music
        public void PlayMusic()
        {
            if (_isMusicPlaying)
                PlayLevelMusic();
        }

        public void PauseMusic()
        {
            PauseLevelMusic();
            PausePowerupMusic();
        }
        public void PlayLevelMusic()
        {
            _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Play();
            _isMusicPlaying = true;
            _isPowerUpPlaying = false;
        }

        public void PauseLevelMusic()
        {
            _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Pause();
        }

        public void PlayPowerupMusic(bool vaccineGrabed)
        {
            if (vaccineGrabed || _isPowerUpPlaying)
            {
                _levelMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Pause();
                _powerupMusic.Play();
                _isMusicPlaying = false;
                _isPowerUpPlaying = true;
            }
        }

        public void PausePowerupMusic()
        {
            if (IsPowerUpPlaying()) _powerupMusic.Pause();
        }

        public bool IsPowerUpPlaying()
        {
            return _powerupMusic.isPlaying;
        }

        public void PlayCinematicMusic()
        {
            _cinematicMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Play();
        }

        public void StopCinematicMusic()
        {
            _cinematicMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].Stop();
        }

        public bool IsCinematicPlaying()
        {
            return _cinematicMusic[(PlayerPrefsManager.GetLevelSelected() - 1)].isPlaying;
        }


        public void PlayEndingMusic()
        {
            _endingMusic.Play();
        }

        public void StopEndingMusic()
        {
            _endingMusic.Stop();
        }

        
        #endregion
    }
}