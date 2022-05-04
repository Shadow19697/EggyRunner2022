using Scripts.Enums;
using Scripts.Managers;
using Scripts.WorldTimeAPI;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Controllers.Principals
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Texture2D _cursorTexture;
        
        private SpecialDateEnum _specialEnum;
        private static Coroutine _uploadRemainingCoroutine;

        void Start()
        {
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            PlayerPrefsManager.InitPlayerPrefs();
            SettingsController.SetVolume(_audioMixer);
            SettingsController.SetVisualSettings(true);
            LocalLoggerManager.InitLocalLoggerManager();
            HttpConnectionManager.Instance.ReturnGames(false);
            if (_uploadRemainingCoroutine != null) StopCoroutine(_uploadRemainingCoroutine);
            StartCoroutine(DataManager.UploadRemainingGames());
            /*******************************************************/
            _specialEnum = SpecialDate.WichSpecialIs();
            Debug.Log("Is First Load? " + PlayerPrefsManager.IsFirstLoad()
                + "\nResolution: " + PlayerPrefsManager.GetWidth() + " x " + PlayerPrefsManager.GetHeight() + " - Resolution Index: " + PlayerPrefsManager.GetResolutionIndex() + " - FullScreen: " + PlayerPrefsManager.IsFullScreen()
                + "\nLevel Selected: " + PlayerPrefsManager.GetLevelSelected() + " - Special Date: " + _specialEnum
                + "\nMusic Value: " + PlayerPrefsManager.GetMusicValue() + " - Sound Value: " + PlayerPrefsManager.GetSoundEffectsValue());
            /*******************************************************/
        }
    }
}
