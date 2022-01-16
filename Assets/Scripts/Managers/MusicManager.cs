using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource _mainMenu;
        public AudioSource _cinematic;
        public AudioSource _ending;
        public AudioSource _gameOver;
        public List<AudioSource> _musicLevel;
        
        public AudioSource _jump;
        public AudioSource _land;
        public AudioSource _benefit;


        public void StopAllMusic()
        {
            _mainMenu.Stop();
            _cinematic.Stop();
            _ending.Stop();
            _musicLevel.ForEach(levelMusic => levelMusic.Stop());
        }
        public void PlayMainMusic()
        {
            StopAllMusic();
            _mainMenu.Play();
        }

        public void PlayCinematicMusic()
        {
            StopAllMusic();
            _cinematic.Play();
        }

        public void PlayEndingMusic()
        {
            StopAllMusic();
            _ending.Play();
        }

        public void PlayLevelMusic(int id)
        {
            StopAllMusic();
            _musicLevel[id].Play();
        }
    }

}