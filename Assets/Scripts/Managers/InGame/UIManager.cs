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
        [SerializeField] private TextMeshProUGUI _lifesCountText;
        [SerializeField] private GameObject _idleUI;
        [SerializeField] private GameObject _playingUI;
        [SerializeField] private GameObject _player;

        private int _eggCount;
        private bool _isPlaying;
        private static UIManager _instance;
        private int _lifesCount;
        private int _scoreMultiplier;
        private float _counter;

        public static UIManager Instance { get {if(_instance == null) _instance = FindObjectOfType<UIManager>(); return _instance; }}

        private void Start()
        {
            _isPlaying = false;
            _actualScoreText.text = "Score: 0";
            _localHighscoreText.text = "Local Highscore: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _globalHighscoreText.text = "Global Highscore: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            _player.SetActive(false);
            _eggCount = 0;
            _lifesCount = 1;
            _scoreMultiplier = 1;
            _counter = 0;
        }

        public void ReturnMenu()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void StartGame()
        {
            _idleUI.SetActive(false);
            //TO DO: ANIMACION QUE SE DESACTIVA EL MENU;
            _isPlaying = true;
            _playingUI.SetActive(true);
            _player.SetActive(true);
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        public void UpdateActualScore()
        {
            _counter += Time.deltaTime * 8 * _scoreMultiplier;
            _actualScoreText.text = "Score: " + (int)_counter;
        }

        public int GetActualScore()
        {
            return (int)_counter;
        }

        public void UpdateEggCount()
        {
            _eggCount++;
            _eggCountText.text = _eggCount.ToString();
        }

        public int GetEggCount()
        {
            return _eggCount;
        }

        public void UpdateLifesCount(int value)
        {
            _lifesCount += value;
            _lifesCountText.text = _lifesCount.ToString();
        }

        public int GetLifesCount()
        {
            return _lifesCount;
        }

        public void UpdateScoreMultiplier(int value)
        {
            _scoreMultiplier = value;
        }

        public void QuitButton()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }
    }
}
