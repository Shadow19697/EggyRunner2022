using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Scripts.Managers;
using Scripts.Models;
using Newtonsoft.Json;

namespace Scripts.Views
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundEffectsSlider;
        [SerializeField] private Dropdown _resolutionDropdown;
        [SerializeField] private Dropdown _qualityDropdown;
        [SerializeField] private Toggle _fullscreenToggle;

        private SettingsWindowsManager _settingsWindows;
        private bool _isSave;
        private SettingsModel _settings = new SettingsModel();
        
        private void Start()
        {
            SettingsInit();
        }

        private void SettingsInit()
        {
            _settingsWindows = GetComponent<SettingsWindowsManager>();
            _musicSlider.value = PlayerPrefsManager.GetMusicValue();
            _soundEffectsSlider.value = PlayerPrefsManager.GetSoundEffectsValue();
            _qualityDropdown.value = PlayerPrefsManager.GetQualityIndex();
            _qualityDropdown.RefreshShownValue();
            _fullscreenToggle.isOn = PlayerPrefsManager.IsFullScreen();
            ResolutionInit();
        }

        private void ResolutionInit()
        {
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(PlayerPrefsManager.ListResolutions);
            _resolutionDropdown.value = PlayerPrefsManager.GetResolutionIndex();
            _resolutionDropdown.RefreshShownValue();
        }

        #region Settings Methods

        public void UpdateMusicVolume(float sliderValue)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            _settings.musicValue = sliderValue;
        }
        public void UpdateSoundEffectsVolume(float sliderValue)
        {
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(sliderValue) * 20);
            _settings.soundEffectsValue = sliderValue;
        }
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            _settings.qualityIndex = qualityIndex;
        }
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            if (isFullscreen) _settings.fullScreen = 1;
            else _settings.fullScreen = 0;
            Debug.LogWarning("FULLSCREEN: " + isFullscreen);
        }
        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = PlayerPrefsManager.Resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            _settings.resolutionIndex = resolutionIndex;
            _settings.height = resolution.height;
            _settings.width = resolution.width;
        }
        public void AddScore()
        {
            PlayerPrefsManager.UpdateTotalScore(750);
        }

        #endregion

        #region Button Methods
        public void ReturnButton()
        {
            Debug.LogWarning(JsonConvert.SerializeObject(_settings));
            Debug.LogWarning(JsonConvert.SerializeObject(PlayerPrefsManager.GetSettingValues()));
            if (JsonConvert.SerializeObject(_settings) != JsonConvert.SerializeObject(PlayerPrefsManager.GetSettingValues()))
                _isSave = _settingsWindows.SaveWindow();
            else
            {
                _settingsWindows.CloseSettings();
                SettingsInit();
            }
        }

        public void EraseButton()
        {
            _isSave = _settingsWindows.EraseWindow();
        }

        public void YesButton()
        {
            if (_isSave)
                PlayerPrefsManager.UpdateSettingsValues(_settings);
            else
                ResetScore();
            _settingsWindows.OpenOkWindow();
        }

        public void NoButton()
        {
            if (_isSave) { 
                RestoreValues();
                _settingsWindows.CloseSettings();
                SettingsInit();
            }
            else
                CancelButton();
        }

        public void CancelButton()
        {
            _settingsWindows.CancelButton();
        }

        public void OkButton()
        {
            _settingsWindows.OkButton(_isSave);
            if (_isSave)
            {
                _settingsWindows.CloseSettings();
                SettingsInit();
            }
        }
        #endregion

        private void RestoreValues()
        {
            UpdateMusicVolume(PlayerPrefsManager.GetMusicValue());
            UpdateSoundEffectsVolume(PlayerPrefsManager.GetSoundEffectsValue());
            SetFullscreen(PlayerPrefsManager.GetFullScreen() == 1 ? true : false);
            SetQuality(PlayerPrefsManager.GetQualityIndex());
            SetResolution(PlayerPrefsManager.GetResolutionIndex());
        }

        public void ResetScore()
        {
            PlayerPrefsManager.ResetTotalScore();
            LocalLoggerManager.ResetLocalLog();
            Debug.LogError("Se borró el puntaje" + PlayerPrefsManager.GetTotalScore());
        }
    }
}