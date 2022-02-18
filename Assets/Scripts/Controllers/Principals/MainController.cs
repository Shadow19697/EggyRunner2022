using Scripts.Enums;
using Scripts.Managers;
using Scripts.WorldTimeAPI;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Controllers.Principals
{
    public class MainController : MonoBehaviour
    {
        public AudioMixer _audioMixer;
        
        private SpecialDateEnum specialEnum;

        void Start()
        {
            PlayerPrefsManager.InitPlayerPrefs();
            SettingsController.SetVolume(_audioMixer);
            SettingsController.SetVisualSettings(true);
            LocalLoggerManager.CreateLocalLog();
            /*******************************************************/
            specialEnum = SpecialDate.WichSpecialIs();
            Debug.Log("Is First Load? " + PlayerPrefsManager.IsFirstLoad()
                + "\nResolution: " + PlayerPrefsManager.GetWidth() + " x " + PlayerPrefsManager.GetHeight() + " - Resolution Index: " + PlayerPrefsManager.GetResolutionIndex() + " - FullScreen: " + PlayerPrefsManager.IsFullScreen()
                + "\nLevel Selected: " + PlayerPrefsManager.GetLevelSelected() + " - Special Date: " + specialEnum
                + "\nMusic Value: " + PlayerPrefsManager.GetMusicValue() + " - Sound Value: " + PlayerPrefsManager.GetSoundEffectsValue()
                + "\nQuality Index: " + PlayerPrefsManager.GetQualityIndex() + " - Total Score: " + PlayerPrefsManager.GetTotalScore());
            /*******************************************************/
        }
    }
}
