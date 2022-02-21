using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Scripts.Managers;

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
        [SerializeField] private RectTransform _resDropdownList;

        private void Start()
        {
            _musicSlider.value = PlayerPrefsManager.GetMusicValue();
            _soundEffectsSlider.value = PlayerPrefsManager.GetSoundEffectsValue();
            _qualityDropdown.value = PlayerPrefsManager.GetQualityIndex();
            _qualityDropdown.RefreshShownValue();
            _fullscreenToggle.isOn = PlayerPrefsManager.IsFullScreen();
            ResolutionInit();
        }

        public void UpdateMusicVolume(float sliderValue)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefsManager.UpdateMusicValue(sliderValue);
        }
        public void UpdateSoundEffectsVolume(float sliderValue)
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
            Resolution resolution = PlayerPrefsManager.Resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefsManager.UpdateResolutionIndex(resolutionIndex);
            PlayerPrefsManager.UpdateHeight(resolution.height);
            PlayerPrefsManager.UpdateWidth(resolution.width);
        }
        private void ResolutionInit()
        {
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(PlayerPrefsManager.ListResolutions);
            _resolutionDropdown.value = PlayerPrefsManager.GetResolutionIndex();
            _resDropdownList.sizeDelta = new Vector2(0, PlayerPrefsManager.ListResolutions.Count * 70);
            _resolutionDropdown.RefreshShownValue();
        }
        public void ResetScore()
        {
            PlayerPrefsManager.ResetTotalScore();
            LocalLoggerManager.ResetLocalLog();
            Debug.LogError("Se borr� el puntaje" + PlayerPrefsManager.GetTotalScore());
        }
        public void AddScore()
        {
            PlayerPrefsManager.UpdateTotalScore(750);
        }
    }
}