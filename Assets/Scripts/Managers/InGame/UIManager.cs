using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine.UI;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UICanvas _uiCanvas;
        [System.Serializable]
        public class UICanvas
        {
            public List<GameObject> _cinematics;
            public GameObject _idleCanvas;
            public GameObject _menuCanvas;
            public GameObject _playingCanvas;
            public GameObject _quitCanvas;
            public GameObject _blackBorders;
        }
        [SerializeField] private IdleText _idleText;
        [System.Serializable]
        public class IdleText
        {
            public TextMeshProUGUI _localHighscoreText;
            public TextMeshProUGUI _globalHighscoreText;
            public TextMeshProUGUI _tipsText;
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
            public TextMeshProUGUI _countdownText;
        }

        [SerializeField] private List<LevelTips> _levelTips;
        
        [System.Serializable]
        public class LevelTips
        {
            public List<GameObject> _tips;
        }

        [SerializeField] private Buttons _buttons;

        [System.Serializable]
        public class Buttons
        {
            public GameObject _pause;
            public GameObject _yes;
            public GameObject _play;
        }

        [SerializeField] private GameObject _player;

        [SerializeField] private PlayingUI _playingUi;
        [System.Serializable]
        public class PlayingUI
        {
            public Sprite _nanosatelliteSprite;
            public Sprite _asteroidSprite;
            public GameObject _collectableImage;
            public GameObject _obstacleGameObject;
            public GameObject _obstacleImage;
        }

        private int _eggCount = 0;
        private int _lifesCount = 1;
        private int _obstacleCount = 0;
        private int _scoreMultiplier = 1;
        private int _timeUpgradeActive = 10;
        private int _indexOfLevel;
        private int _indexOfTips = 0;

        private bool _isCinematic = true;
        private bool _isPlaying = false;
        private bool _isPaused = false;
        private bool _isGameOver = false;
        private bool _gettingScore;
        private bool _immunityActivated;

        private Color32 _white = new Color32(255, 255, 255, 255);
        private Color32 _cian = new Color32(91, 217, 231, 255);
        private Color32 _orange = new Color32(228, 194, 13, 255);

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
            _buttons._pause.SetActive(false);
            _playingText._upgradeText.text = "";
            _idleText._globalHighscoreText.text = "";
            _idleText._localHighscoreText.text = "";
            _playingText._eggCountText.text = "x0";
            _playingText._lifesCountText.text = "x1";
            _playingText._obstacleCountText.text = "x0";
            _indexOfLevel = PlayerPrefsManager.GetLevelSelected() - 1;
            if(_indexOfLevel > 2) _playingUi._obstacleGameObject.SetActive(false);
            if (_indexOfLevel == 2)
            {
                _playingUi._collectableImage.GetComponent<Image>().sprite = _playingUi._nanosatelliteSprite;
                _playingUi._obstacleImage.GetComponent<Image>().sprite = _playingUi._asteroidSprite;
            }
            _uiCanvas._cinematics.ForEach(cinematic => cinematic.SetActive(false));
            _levelTips.ForEach(level => level._tips.ForEach(tip => tip.SetActive(false)));
            _uiCanvas._menuCanvas.SetActive(false);
            _uiCanvas._playingCanvas.SetActive(false);
            _uiCanvas._cinematics[_indexOfLevel].SetActive(true);
            _uiCanvas._blackBorders.SetActive(true);
        }

        private void GetGlobalHighscore(bool isStart)
        {
            try
            {
                _idleText._globalHighscoreText.text = "Mejor Puntaje\nGlobal: " + DataManager.GetGlobalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
                _gettingScore = false;
            }
            catch
            {
                if (isStart)
                {
                    HttpConnectionManager.Instance.ReturnGames(false);
                    _gettingScore = true;
                }
                _idleText._globalHighscoreText.text = "Esperando\npuntaje global...";
            }
        }

        [Obsolete]
        private void Update()
        {
            if (_isCinematic)
            {
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1") || !SoundManager.Instance.IsCinematicPlaying() || PlayerPrefsManager.WasTheCinematicSeen())
                    DisplayIdleCanvas();
            }
            else
            {
                if (_uiCanvas._idleCanvas.active)
                {
                    if (Input.GetButtonDown("Cancel") && !_uiCanvas._quitCanvas.active) OpenQuitCanvas();
                    else if (Input.GetButtonDown("Cancel") && _uiCanvas._quitCanvas.active) NoButton();
                }
                if (_uiCanvas._playingCanvas.active && !IsGameOver())
                {
                    if (Input.GetButtonDown("Cancel") && !_uiCanvas._menuCanvas.active) OpenMenuCanvas();
                    else if (Input.GetButtonDown("Cancel") && _uiCanvas._menuCanvas.active) StartGame();
                }
                if (_gettingScore)
                    GetGlobalHighscore(false);
                if (IsGameOver()) _buttons._pause.SetActive(false);
            }
        }

        private void DisplayIdleCanvas()
        {
            PlayerPrefsManager.UpdateCinematicSeen(true);
            _isCinematic = false;
            _uiCanvas._cinematics[_indexOfLevel].SetActive(false);
            _uiCanvas._blackBorders.SetActive(false);
            _uiCanvas._idleCanvas.SetActive(true);
            _uiCanvas._menuCanvas.SetActive(true);
            _levelTips[_indexOfLevel]._tips[_indexOfTips].SetActive(true);
            _idleText._tipsText.text = "CONSEJOS (" + (_indexOfTips + 1) + "/" + (_levelTips[_indexOfLevel]._tips.Count) + ")"; 
            _idleText._localHighscoreText.text = "Mejor Puntaje\nLocal: " + DataManager.GetLocalHighscoreOfLevel(PlayerPrefsManager.GetLevelSelected());
            GetGlobalHighscore(true);
        }

        public bool IsCinematicPlaying()
        {
            return _isCinematic;
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
            switch (_scoreMultiplier)
            {
                case 2: _playingText._actualScoreText.color = _cian;
                    break;
                case 3: _playingText._actualScoreText.color = _orange;
                    break;
                default: _playingText._actualScoreText.color = _white;
                    break;
            }
            _scoreCounter += (_indexOfLevel + 1 != 3) ? Time.deltaTime * 9 * _scoreMultiplier : Time.deltaTime * 9 * _scoreMultiplier * 1.7f;
            _playingText._actualScoreText.text = "Puntaje: " + (int)_scoreCounter;
        }

        public int GetActualScore()
        {
            return (int)_scoreCounter;
        }

        public void SetWhiteScoreText()
        {
            _playingText._actualScoreText.color = _white;
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
            if (_lifesCount + value >= 0)
            {
                _lifesCount += value;
                _playingText._lifesCountText.text = "x" + _lifesCount.ToString();
            }
            if (_lifesCount <= 0)
            {
                _isGameOver = true;
                ObjectsManager.Instance.EnableDamageCollider(false);
            }
        }

        public bool IsPlayerAlive()
        {
            return !_isGameOver;
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
            SoundManager.Instance.PlayPowerupMusic(true);
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
                _buttons._pause.SetActive(true);
                _uiCanvas._idleCanvas.SetActive(false);
                _uiCanvas._menuCanvas.SetActive(false);
                _isPlaying = true;
                _isPaused = false;
                _uiCanvas._playingCanvas.SetActive(true);
                _player.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void OpenQuitCanvas()
        {
            _uiCanvas._quitCanvas.SetActive(true);
            SetSelectedButton(_buttons._yes);
        }

        public void QuitApplication()
        {
            Debug.LogError("Quit");
            Application.Quit();
        }

        public void NoButton()
        {
            _uiCanvas._quitCanvas.SetActive(false);
            SetSelectedButton(_buttons._play);
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
            _idleText._tipsText.text = "CONSEJOS (" + (_indexOfTips + 1) + "/" + (_levelTips[_indexOfLevel]._tips.Count) + ")";
        }

        public void OpenMenuCanvas()
        {
            SoundManager.Instance.PausePowerupMusic();
            _isPaused = true;
            _buttons._pause.SetActive(false);
            _uiCanvas._menuCanvas.SetActive(true);
            SetSelectedButton(_buttons._play);
            Time.timeScale = 0;
        }
        #endregion

        private IEnumerator ResumeGameDelayed()
        {
            _playingText._countdownText.gameObject.SetActive(true);
            _uiCanvas._menuCanvas.SetActive(false);
            for (int i = 3; i > 0; i--)
            {
                _playingText._countdownText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1);
            }
            _isPlaying = false;
            _playingText._countdownText.text = "";
            SoundManager.Instance.PlayPowerupMusic(false);
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
                    SoundManager.Instance.PauseMusic();
                }
                else
                    SoundManager.Instance.PlayMusic(); 
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (IsPlaying() && !IsGameOver())
            {
                if (pauseStatus)
                {
                    OpenMenuCanvas();
                    SoundManager.Instance.PauseMusic();
                }
                else
                    SoundManager.Instance.PlayMusic(); 
            }
        }
    }
}
