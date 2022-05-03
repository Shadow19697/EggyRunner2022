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
        private static Coroutine _uploadRemainingCoroutine;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerPrefsManager.InitPlayerPrefs();
            SettingsController.SetVolume(_audioMixer);
            SettingsController.SetVisualSettings(true);
            LocalLoggerManager.InitLocalLoggerManager();
            HttpConnectionManager.Instance.ReturnGames(false);
            if (_uploadRemainingCoroutine != null) StopCoroutine(_uploadRemainingCoroutine);
            StartCoroutine(DataManager.UploadRemainingGames());
            /*******************************************************/
            specialEnum = SpecialDate.WichSpecialIs();
            Debug.Log("Is First Load? " + PlayerPrefsManager.IsFirstLoad()
                + "\nResolution: " + PlayerPrefsManager.GetWidth() + " x " + PlayerPrefsManager.GetHeight() + " - Resolution Index: " + PlayerPrefsManager.GetResolutionIndex() + " - FullScreen: " + PlayerPrefsManager.IsFullScreen()
                + "\nLevel Selected: " + PlayerPrefsManager.GetLevelSelected() + " - Special Date: " + specialEnum
                + "\nMusic Value: " + PlayerPrefsManager.GetMusicValue() + " - Sound Value: " + PlayerPrefsManager.GetSoundEffectsValue());
            /*******************************************************/
        }
    }
}
