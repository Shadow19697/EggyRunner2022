using Scripts.Managers;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Controllers
{
    public static class SettingsController
    {
        public static void SetVolume(AudioMixer _audioMixer)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefsManager.GetMusicValue()) * 20);
            _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(PlayerPrefsManager.GetSoundEffectsValue()) * 20);
        }

        public static void SetVisualSettings(bool isMenu)
        {
            Screen.fullScreen = PlayerPrefsManager.IsFullScreen();
            if (isMenu) SetResolutionSettings();
            else SetResolutionSaved();
        }

        private static void SetResolutionSettings()
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
            }
            else
                SetResolutionSaved();
        }

        private static void SetResolutionSaved()
        {
            Screen.SetResolution(PlayerPrefsManager.GetWidth(), PlayerPrefsManager.GetHeight(), Screen.fullScreen);
        }
    } 
}
