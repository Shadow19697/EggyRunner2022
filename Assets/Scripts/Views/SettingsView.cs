using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Scripts.Managers;
using Scripts.Models;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

namespace Scripts.Views
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundEffectsSlider;
        [SerializeField] private Dropdown _resolutionDropdown;
        [SerializeField] private Toggle _fullscreenToggle;
        [SerializeField] private GameObject _eraseButton;
        [SerializeField] private GameObject _returnButton;

        private SettingsWindowsManager _settingsWindows;
        private bool _isSave;
        private SettingsModel _settings = new SettingsModel();
        private bool _isReturn;
        
        private void Start()
        {
            SettingsInit();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                ReturnButton();
        }

        private void SettingsInit()
        {
            _settingsWindows = GetComponent<SettingsWindowsManager>();
            _musicSlider.value = PlayerPrefsManager.GetMusicValue();
            _soundEffectsSlider.value = PlayerPrefsManager.GetSoundEffectsValue();
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
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            if (isFullscreen) _settings.fullScreen = 1;
            else _settings.fullScreen = 0;
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
            if (JsonConvert.SerializeObject(_settings) != JsonConvert.SerializeObject(PlayerPrefsManager.GetSettingValues()))
            {
                _isReturn = true;
                _isSave = _settingsWindows.SaveWindow();
            }
            else
            {
                _settingsWindows.CloseSettings();
                SettingsInit();
            }
        }

        public void EraseButton()
        {
            _isReturn = false;
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
            SelectLastButton();
        }

        public void OkButton()
        {
            _settingsWindows.OkButton(_isSave);
            if (_isSave)
            {
                _settingsWindows.CloseSettings();
                SettingsInit();
            }
            else
                SelectLastButton();
        }

        private void SelectLastButton()
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (_isReturn)
                EventSystem.current.SetSelectedGameObject(_returnButton);
            else
                EventSystem.current.SetSelectedGameObject(_eraseButton);
        }
        #endregion

        private void RestoreValues()
        {
            UpdateMusicVolume(PlayerPrefsManager.GetMusicValue());
            UpdateSoundEffectsVolume(PlayerPrefsManager.GetSoundEffectsValue());
            SetFullscreen(PlayerPrefsManager.GetFullScreen() == 1 ? true : false);
            SetResolution(PlayerPrefsManager.GetResolutionIndex());
        }

        public void ResetScore()
        {
            PlayerPrefsManager.ResetTotalScore();
            LocalLoggerManager.ResetLocalGamesLog();
            Debug.LogWarning("Se borraron las partidas y el puntaje:" + PlayerPrefsManager.GetTotalScore());
        }
    }
}