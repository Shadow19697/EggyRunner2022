using UnityEngine;
using TMPro;
using Scripts.Managers;
using UnityEngine.EventSystems;
using Scripts.Controllers.Extensions;

namespace Scripts.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelScoreText;
        [SerializeField] private TextMeshProUGUI _settingsScoreText;
        [SerializeField] private GameObject _mainMenuObject;
        [SerializeField] private GameObject _levelMenuObject;
        [SerializeField] private GameObject _settingsMenuObject;
        [SerializeField] private GameObject _quitCanvas;
        [SerializeField] private GameObject _yesButton;

        private void Start()
        {
            if (PlayerPrefsManager.GetLevelSelected() != 0)
            {
                _mainMenuObject.SetActive(false);
                _levelMenuObject.SetActive(true);
                PlayerPrefsManager.UpdateLevelSelected(0);
            }
        }

        [System.Obsolete]
        private void Update()
        {
            if (_levelMenuObject.active)
                _levelScoreText.text = "Puntos: " + PlayerPrefsManager.GetTotalScore();
            if (_settingsMenuObject.active)
                _settingsScoreText.text = "Puntos: " + PlayerPrefsManager.GetTotalScore();
            if (_mainMenuObject.active)
            {
                if (Input.GetButtonDown("Cancel") && !_quitCanvas.active) OpenQuitCanvas();
                else if (Input.GetButtonDown("Cancel") && _quitCanvas.active) NoButton();
            }
        }

        public void OpenQuitCanvas()
        {
            _quitCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_yesButton);
        }

        public void QuitApplication()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }

        public void NoButton()
        {
            _quitCanvas.SetActive(false);
            NavigationMenuController.Instance.ReturnToMainMenu();
        }

        /**************************************************************/
        public void ToFirstLoad()
        {
            PlayerPrefsManager.ResetFirstLoad();
        }
        /**************************************************************/
    } 
}
