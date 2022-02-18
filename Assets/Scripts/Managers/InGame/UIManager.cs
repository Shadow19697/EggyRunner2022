using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI _localHighscoreText;
        public TextMeshProUGUI _globalHighscoreText;

        private void Start()
        {
            _localHighscoreText.text = "Local Highscore: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _globalHighscoreText.text = "Global Highscore: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
        }

        public void ReturnMenu()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
