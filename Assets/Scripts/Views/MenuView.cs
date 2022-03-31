using UnityEngine;
using TMPro;
using Scripts.Managers;

namespace Scripts.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelScoreText;
        [SerializeField] private TextMeshProUGUI _settingsScoreText;
        [SerializeField] private GameObject _mainMenuObject;
        [SerializeField] private GameObject _levelMenuObject;
        [SerializeField] private GameObject _settingsMenuObject;

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
        }

        public void QuitButton()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }

        /**************************************************************/
        public void ToFirstLoad()
        {
            PlayerPrefsManager.ResetFirstLoad();
        }
        /**************************************************************/
    } 
}
