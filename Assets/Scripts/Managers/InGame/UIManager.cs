using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _actualScoreText;
        [SerializeField] private TextMeshProUGUI _localHighscoreText;
        [SerializeField] private TextMeshProUGUI _globalHighscoreText;
        [SerializeField] private GameObject _idleUI;
        [SerializeField] private GameObject _playingUI;
        [SerializeField] private GameObject _player;

        private bool isPlaying;

        private void Start()
        {
            isPlaying = false;
            _actualScoreText.text = "Score: 0";
            _localHighscoreText.text = "Local Highscore: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _globalHighscoreText.text = "Global Highscore: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _player.SetActive(false);
        }

        public void ReturnMenu()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void StartGame()
        {
            _idleUI.SetActive(false);
            //TO DO: ANIMACION QUE SE DESACTIVA EL MENU;
            isPlaying = true;
            _playingUI.SetActive(true);
            _player.SetActive(true);
        }

        public bool IsPlaying()
        {
            return isPlaying;
        }

        public void UpdateActualScore(int score)
        {
            _actualScoreText.text = "Score: " + score;
        }

        public void QuitButton()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }
    }
}
