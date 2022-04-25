using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _actualScoreText;
        [SerializeField] private TextMeshProUGUI _localHighscoreText;
        [SerializeField] private TextMeshProUGUI _globalHighscoreText;
        [SerializeField] private TextMeshProUGUI _eggCountText;
        [SerializeField] private TextMeshProUGUI _lifesCountText;
        [SerializeField] private TextMeshProUGUI _upgradeText;
        [SerializeField] private GameObject _idleUI;
        [SerializeField] private GameObject _playingUI;
        [SerializeField] private GameObject _quitCanvas;
        [SerializeField] private GameObject _yesButton;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _player;


        private int _eggCount;
        private bool _isPlaying;
        private static UIManager _instance;
        private int _lifesCount;
        private int _scoreMultiplier;
        private float _scoreCounter;
        private bool _gettingScore;

        public static UIManager Instance { get {if(_instance == null) _instance = FindObjectOfType<UIManager>(); return _instance; }}

        private void Start()
        {
            _isPlaying = false;
            _player.SetActive(false);
            _eggCount = 0;
            _lifesCount = 1;
            _scoreMultiplier = 1;
            _scoreCounter = 0;
            _upgradeText.text = "";
            _localHighscoreText.text = "Mejor Puntaje Local: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            try
            {
                _globalHighscoreText.text = "Mejor Puntaje Global: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
                _gettingScore = false;
            }
            catch
            {
                HttpConnectionManager.Instance.ReturnGames(false);
                _gettingScore = true;
                _globalHighscoreText.text = "Esperando puntaje global...";
            }
        }

        [Obsolete]
        private void Update()
        {
            if (_idleUI.active)
            {
                if (Input.GetButtonDown("Cancel") && !_quitCanvas.active) OpenQuitCanvas();
                else if (Input.GetButtonDown("Cancel") && _quitCanvas.active) NoButton();
            }
            if (_gettingScore)
            {
                try
                {
                    _globalHighscoreText.text = "Mejor Puntaje Global: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
                    _gettingScore = false;
                }
                catch
                {
                    _globalHighscoreText.text = "Esperando puntaje global...";
                }
            }
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

        #region Actual Score Methods
        public void UpdateActualScore()
        {
            _scoreCounter += Time.deltaTime * 8 * _scoreMultiplier;
            _actualScoreText.text = "Puntaje: " + (int)_scoreCounter;
        }

        public int GetActualScore()
        {
            return (int)_scoreCounter;
        }
        #endregion

        #region Egg Count Methods
        public void UpdateEggCount()
        {
            _eggCount++;
            _eggCountText.text = "x" + _eggCount.ToString();
        }

        public int GetEggCount()
        {
            return _eggCount;
        }
        #endregion

        #region Lifes Count Methods
        public void UpdateLifesCount(int value)
        {
            _lifesCount += value;
            _lifesCountText.text = "x" + _lifesCount.ToString();
        }

        public int GetLifesCount()
        {
            return _lifesCount;
        } 
        #endregion

        public void UpdateScoreMultiplier(int value)
        {
            _scoreMultiplier = value;
            _upgradeText.text = ("x" + value);
            StartCoroutine(UpdateScoreMultiplier());
        }

        private IEnumerator UpdateScoreMultiplier()
        {
            yield return new WaitForSeconds(20);
            _scoreMultiplier = 1;
            _upgradeText.text = "";
        }

        public void DisplayImmortality()
        {
            _upgradeText.text = "Inmortalidad";
            StartCoroutine(DisplayImmortalityUntil());
        }

        private IEnumerator DisplayImmortalityUntil()
        {
            yield return new WaitForSeconds(20);
            _upgradeText.text = "";
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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_playButton);
        }
    }
}
