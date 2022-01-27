using Scripts.Enums;
using Scripts.Managers;
using Scripts.Managers.Sounds;
using Scripts.WorldTimeAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Controllers
{
    public class MainController : MonoBehaviour
    {
        public AudioMixer _audioMixer;
        
        private SpecialDateEnum specialEnum;

        void Start()
        {
            SetVolume();
            SetVisualSettings();
            LocalLoggerManager.CreateLocalLog();
            specialEnum = SpecialDate.WichSpecialIs();
            Debug.Log("Is First Load? " + PlayerPrefsManager.IsFirstLoad() + " - Is From Playing? " + PlayerPrefsManager.IsFromPlaying()
                + "\nResolution: " + PlayerPrefsManager.GetWidth() + " x " + PlayerPrefsManager.GetHeight() + " - Resolution Index: " + PlayerPrefsManager.GetResolutionIndex() + " - FullScreen: " + PlayerPrefsManager.IsFullScreen()
                + "\nLevel Selected: " + PlayerPrefsManager.GetLevelSelected() + " - Special Date: " + specialEnum
                + "\nMusic Value: " + PlayerPrefsManager.GetMusicValue() + " - Sound Value: " + PlayerPrefsManager.GetSoundEffectsValue()
                + "\nQuality Index: " + PlayerPrefsManager.GetQualityIndex() + " - Total Score: " + PlayerPrefsManager.GetTotalScore());
            PlayerPrefsManager.InitPlayerPrefs();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetVolume()
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        public void SetVisualSettings()
        {
            QualitySettings.SetQualityLevel(PlayerPrefsManager.GetQualityIndex());
            Screen.fullScreen = PlayerPrefsManager.IsFullScreen();
            SetResolutionSettings();
        }

        private void SetResolutionSettings()
        {
            PlayerPrefsManager.Resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;
            for (int i = 0; i < PlayerPrefsManager.Resolutions.Length; i++)
            {
                string option = PlayerPrefsManager.Resolutions[i].width + " x " + PlayerPrefsManager.Resolutions[i].height;
                PlayerPrefsManager.ListResolutions.Add(option);
                if (PlayerPrefsManager.Resolutions[i].width == Screen.currentResolution.width && PlayerPrefsManager.Resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }
           if (PlayerPrefsManager.IsFirstLoad())
            {
                Resolution resolution = PlayerPrefsManager.Resolutions[currentResolutionIndex];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                PlayerPrefsManager.UpdateResolutionIndex(currentResolutionIndex);
                PlayerPrefsManager.UpdateHeight(resolution.height);
                PlayerPrefsManager.UpdateWidth(resolution.width);
                PlayerPrefsManager.UpdateFirstLoad();
                //****************************************************************************
                //PlayerPrefsManager.UpdateTotalScore(1000);
                //****************************************************************************
            }
            else
            {
                //****************************************************************************
                //PlayerPrefsManager.ResetFirstLoad();
                //****************************************************************************
                Screen.SetResolution(PlayerPrefsManager.GetWidth(), PlayerPrefsManager.GetHeight(), Screen.fullScreen);
            }
        }
    }
}
