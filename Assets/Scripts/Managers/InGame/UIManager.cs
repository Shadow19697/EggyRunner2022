using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Scripts.Player;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UICanvas _uiCanvas;
        [System.Serializable]
        public class UICanvas
        {
            public GameObject _idleCanvas;
            public GameObject _menuCanvas;
            public GameObject _playingCanvas;
            public GameObject _quitCanvas;
        }
        [SerializeField] private IdleText _idleText;
        [System.Serializable]
        public class IdleText
        {
            public TextMeshProUGUI _localHighscoreText;
            public TextMeshProUGUI _globalHighscoreText;
        }

        [SerializeField] private PlayingText _playingText;
        [System.Serializable]
        public class PlayingText
        {
            public TextMeshProUGUI _actualScoreText;
            public TextMeshProUGUI _lifesCountText;
            public TextMeshProUGUI _eggCountText;
            public TextMeshProUGUI _obstacleCountText;
            public TextMeshProUGUI _upgradeText;
        }

        [SerializeField] private List<LevelTips> _levelTips;
        
        [System.Serializable]
        public class LevelTips
        {
            public List<GameObject> _tips;
        }

        [SerializeField] private GameObject _yesButton;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _player;    
        
        private int _eggCount = 0;
        private int _lifesCount = 1;
        private int _obstacleCount = 0;
        private int _scoreMultiplier = 1;
        private int _timeUpgradeActive = 10;
        private int _indexOfLevel;
        private int _indexOfTips = 0;

        private bool _isPlaying = false;
        private bool _isPaused = false;
        private bool _isGameOver = false;
        private bool _gettingScore;
        private bool _immunityActivated;

        private float _scoreCounter = 0;

        private static UIManager _instance;

        public static UIManager Instance { get {if(_instance == null) _instance = FindObjectOfType<UIManager>(); return _instance; }}

        private void Start()
        {
            SetValues();
        }

        private void SetValues()
        {
            _player.SetActive(false);
            _playingText._upgradeText.text = "";
            _indexOfLevel = PlayerPrefsManager.GetLevelSelected() - 1;
            _levelTips.ForEach(level => level._tips.ForEach(tip => tip.SetActive(false)));
            _levelTips[_indexOfLevel]._tips[_indexOfTips].SetActive(true);
            _idleText._localHighscoreText.text = "Mejor Puntaje\nLocal: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            GetGlobalHighscore(true);
        }

        [Obsolete]
        private void Update()
        {
            if (_uiCanvas._idleCanvas.active) {
                if (Input.GetButtonDown("Cancel") && !_uiCanvas._quitCanvas.active) OpenQuitCanvas();
                else if (Input.GetButtonDown("Cancel") && _uiCanvas._quitCanvas.active) NoButton();
            }
            if (_uiCanvas._playingCanvas.active)
            {
                if (Input.GetButtonDown("Cancel") && !_uiCanvas._menuCanvas.active) OpenMenuCanvas();
                else if (Input.GetButtonDown("Cancel") && _uiCanvas._menuCanvas.active) StartGame();
            }
            if (_gettingScore)
                GetGlobalHighscore(false);
        }

        private void GetGlobalHighscore(bool isStart)
        {
            try {
                _idleText._globalHighscoreText.text = "Mejor Puntaje\nGlobal: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
                _gettingScore = false;
            }
            catch {
                if (isStart) {
                    HttpConnectionManager.Instance.ReturnGames(false);
                    _gettingScore = true;
                }
                _idleText._globalHighscoreText.text = "Esperando\npuntaje global...";
            }
        }        

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }

        public bool IsPaused()
        {
            return _isPaused;
        }

        #region Actual Score Methods
        public void UpdateActualScore()
        {
            _scoreCounter += Time.deltaTime * 9 * _scoreMultiplier;
            _playingText._actualScoreText.text = "Puntaje: " + (int)_scoreCounter;
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
            _playingText._eggCountText.text = "x" + _eggCount.ToString();
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
            _playingText._lifesCountText.text = "x" + _lifesCount.ToString();
            if (_lifesCount == 0) _isGameOver = true;
        }

        public bool IsPlayerAlive()
        {
            return (_lifesCount!=0);
        }
        #endregion

        #region Score Multiplier Methods
        public void UpdateScoreMultiplier(int value)
        {
            _scoreMultiplier = value;
            _playingText._upgradeText.text = ("x" + value);
            StartCoroutine(UpdateScoreMultiplier());
            StartCoroutine(Fade(_playingText._upgradeText, _timeUpgradeActive));
        }

        private IEnumerator UpdateScoreMultiplier()
        {
            yield return new WaitForSeconds(_timeUpgradeActive);
            _scoreMultiplier = 1;
            _playingText._upgradeText.text = "";
        }
        #endregion

        #region Immunity Methods
        public void DisplayImmunity()
        {
            _playingText._upgradeText.text = "INMUNIDAD";
            StartCoroutine(DisplayImmunityUntil(_timeUpgradeActive+4));
            StartCoroutine(Fade(_playingText._upgradeText, _timeUpgradeActive+4));
            StartCoroutine(PlayerController.Instance.GlowAnimation());
            SoundManager.Instance.PlayPowerupMusic();
        }

        private IEnumerator DisplayImmunityUntil(float timeActive)
        {
            ObjectsManager.Instance.EnableDamageCollider(false);
            _immunityActivated = true;
            yield return new WaitForSeconds(timeActive);
            _playingText._upgradeText.text = "";
            ObjectsManager.Instance.EnableDamageCollider(true);
            _immunityActivated = false;
        }

        public bool IsImmunityActivated()
        {
            return _immunityActivated;
        }
        #endregion

        private IEnumerator Fade(TextMeshProUGUI text, float fadeTime)
        {
            text.color = new Color(1, 1, 1, 1);
            float alpha = text.color.a;
            for(float t = 0.0f; t < 1.0f; t+= Time.deltaTime / fadeTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
                text.color = newColor;
                yield return null;
            }
        }

        #region Obstacle Methods
        public void AddObstacleCount()
        {
            _obstacleCount++;
            _playingText._obstacleCountText.text = "x" + _obstacleCount.ToString();
        }

        public int GetObstaclesCount()
        {
            return _obstacleCount;
        }
        #endregion

        #region Buttons Methods
        public void StartGame()
        {
            if (_isPlaying)
                StartCoroutine(ResumeGameDelayed());
            else
            {
                Time.timeScale = 1;
                _uiCanvas._idleCanvas.SetActive(false);
                _uiCanvas._menuCanvas.SetActive(false);
                _isPlaying = true;
                _isPaused = false;
                _uiCanvas._playingCanvas.SetActive(true);
                _player.SetActive(true);
            }
        }

        public void OpenQuitCanvas()
        {
            _uiCanvas._quitCanvas.SetActive(true);
            SetSelectedButton(_yesButton);
        }

        public void QuitApplication()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }

        public void NoButton()
        {
            _uiCanvas._quitCanvas.SetActive(false);
            SetSelectedButton(_playButton);
        }

        public void ReturnMenu()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void SideButton(bool isPrev)
        {
            _levelTips[_indexOfLevel]._tips[_indexOfTips].SetActive(false);
            if (isPrev)
            {
                if (_indexOfTips > 0) _indexOfTips--;
                else _indexOfTips = _levelTips[_indexOfLevel]._tips.Count - 1;
            }
            else
            {
                if (_indexOfTips < _levelTips[_indexOfLevel]._tips.Count - 1) _indexOfTips++;
                else _indexOfTips = 0;
            }
            _levelTips[_indexOfLevel]._tips[_indexOfTips].SetActive(true);
        }

        public void OpenMenuCanvas()
        {
            _isPaused = true;
            _uiCanvas._menuCanvas.SetActive(true);
            SetSelectedButton(_playButton);
            Time.timeScale = 0;
        }
        #endregion

        private IEnumerator ResumeGameDelayed()
        {
            _playingText._upgradeText.gameObject.SetActive(true);
            _uiCanvas._menuCanvas.SetActive(false);
            for (int i = 3; i > 0; i--)
            {
                _playingText._upgradeText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1);
            }
            _isPlaying = false;
            _playingText._upgradeText.text = "";
            StartGame();
        }

        private void SetSelectedButton(GameObject button)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (IsPlaying() && !IsGameOver())
            {
                if (!hasFocus)
                {
                    OpenMenuCanvas();
                    SoundManager.Instance.PauseLevelMusic();
                }
                else
                    SoundManager.Instance.PlayLevelMusic(); 
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (IsPlaying() && !IsGameOver())
            {
                if (pauseStatus)
                {
                    OpenMenuCanvas();
                    SoundManager.Instance.PauseLevelMusic();
                }
                else
                    SoundManager.Instance.PlayLevelMusic(); 
            }
        }
    }
}
