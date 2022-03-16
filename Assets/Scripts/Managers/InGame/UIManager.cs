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
        [SerializeField] private TextMeshProUGUI _eggCountText;
        [SerializeField] private GameObject _idleUI;
        [SerializeField] private GameObject _playingUI;
        [SerializeField] private GameObject _player;

        private int _eggCount;
        private bool isPlaying;
        private static UIManager instance;
        public static UIManager Instance { get {if(instance == null) instance = FindObjectOfType<UIManager>(); return instance; }}

        private void Start()
        {
            isPlaying = false;
            _actualScoreText.text = "Score: 0";
            _localHighscoreText.text = "Local Highscore: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _globalHighscoreText.text = "Global Highscore: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _player.SetActive(false);
            _eggCount = 0;
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

        public void UpdateEggCount()
        {
            _eggCount++;
            _eggCountText.text = _eggCount.ToString();
        }

        public void QuitButton()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }
    }
}
