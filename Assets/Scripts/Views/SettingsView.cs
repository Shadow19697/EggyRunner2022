using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Scripts.Managers;

namespace Scripts.Views
{
    public class SettingsView : MonoBehaviour
    {
        public AudioMixer _audioMixer;
        public Slider _musicSlider;
        public Slider _soundEffectsSlider;
        public Dropdown _resolutionDropdown;
        public Dropdown _qualityDropdown;
        public Toggle _fullscreenToggle;
        Resolution[] _resolutions;

        private void Start()
        {
            _musicSlider.value = PlayerPrefsManager.GetMusicValue();
            _soundEffectsSlider.value = PlayerPrefsManager.GetSoundEffectsValue();
            _qualityDropdown.value = PlayerPrefsManager.GetQualityIndex();
            _qualityDropdown.RefreshShownValue();
            _fullscreenToggle.isOn = PlayerPrefsManager.IsFullScreen();
            ResolutionInit();
        }
        public void updateMusicVolume(float sliderValue)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefsManager.UpdateMusicValue(sliderValue);
        }
        public void updateSoundEffectsVolume(float sliderValue)
        {
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefsManager.UpdateSoundEffectsValue(sliderValue);
        }

        public void SetQuality (int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefsManager.UpdateQualityIndex(qualityIndex);
        }

        public void SetFullscreen (bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            if (isFullscreen) PlayerPrefsManager.UpdateFullScreen(1);
            else PlayerPrefsManager.UpdateFullScreen(0);
        }

        public void SetResolution (int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefsManager.UpdateResolutionIndex(resolutionIndex);
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
            if (PlayerPrefsManager.IsFirstLoad())
            {
                _resolutionDropdown.value = currentResolutionIndex;
                PlayerPrefsManager.UpdateResolutionIndex(currentResolutionIndex);
                _resolutionDropdown.RefreshShownValue();
                PlayerPrefsManager.UpdateFirstLoad();
            }
            else
            {
                _resolutionDropdown.value = PlayerPrefsManager.GetResolutionIndex();
                _resolutionDropdown.RefreshShownValue();
            }
        }
    }
}