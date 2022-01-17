using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource _mainMenuMusic;
        public AudioSource _cinematicMusic;
        public AudioSource _endingMusic;
        public List<AudioSource> _levelMusic;

        public AudioSource _gameOverSound;
        public AudioSource _jumpSound;
        public AudioSource _landSound;
        public AudioSource _benefitSound;


        public void StopAllMusic()
        {
            _mainMenuMusic.Stop();
            _cinematicMusic.Stop();
            _endingMusic.Stop();
            _levelMusic.ForEach(levelMusic => levelMusic.Stop());
        }
        public void PlayMainMusic()
        {
            StopAllMusic();
            _mainMenuMusic.Play();
        }

        public void PlayCinematicMusic()
        {
            StopAllMusic();
            _cinematicMusic.Play();
        }

        public void PlayEndingMusic()
        {
            StopAllMusic();
            _endingMusic.Play();
        }

        public void PlayLevelMusic(int id)
        {
            StopAllMusic();
            _levelMusic[id].Play();
        }
    }

}