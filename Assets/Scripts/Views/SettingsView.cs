using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace Scripts.Views
{
    public class SettingsView : MonoBehaviour
    {
        public AudioMixer _audioMixer;
        public Slider _musicSlider;
        public Slider _soundEffectsSlider;
        public Dropdown _resolutionDropdown;
        Resolution[] _resolutions;

        private void Start()
        {
            _musicSlider.value = PlayerPrefs.GetFloat("MusicValue", 1f);
            _soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsValue", 1f);
            ResolutionInit();
        }
        public void updateMusicVolume(float sliderValue)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("MusicValue", sliderValue);
        }
        public void updateSoundEffectsVolume(float sliderValue)
        {
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("SoundEffectsValue", sliderValue);
        }

        public void SetQuality (int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen (bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void SetResolution (int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        private void ResolutionInit()
        {
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);
                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }
    }
}