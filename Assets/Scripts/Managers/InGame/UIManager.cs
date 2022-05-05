using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UICanvas _uiCanvas;
        [System.Serializable]
        public class UICanvas
        {
            public GameObject _idleCanvas;
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

        [SerializeField] private List<GameObject> _tipsImages;

        [SerializeField] private GameObject _yesButton;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _player;    
        
        private int _eggCount;
        private bool _isPlaying;
        private bool _isGameOver;
        private static UIManager _instance;
        private int _lifesCount;
        private int _obstacleCount;
        private int _scoreMultiplier;
        private float _scoreCounter;
        private bool _gettingScore;
        private bool _immunityActivated;
        private int _timeUpgradeActive = 10;

        public static UIManager Instance { get {if(_instance == null) _instance = FindObjectOfType<UIManager>(); return _instance; }}

        private void Start()
        {
            SetValues();
        }

        private void SetValues()
        {
            _isPlaying = false;
            _isGameOver = false;
            _player.SetActive(false);
            _eggCount = 0;
            _lifesCount = 1;
            _obstacleCount = 0;
            _scoreMultiplier = 1;
            _scoreCounter = 0;
            _playingText._upgradeText.text = "";
            _tipsImages.ForEach(tip => tip.SetActive(false));
            _tipsImages[PlayerPrefsManager.GetLevelSelected()-1].SetActive(true);
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

        public int GetLifesCount()
        {
            return _lifesCount;
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
            StartCoroutine(DisplayImmunityUntil());
            StartCoroutine(Fade(_playingText._upgradeText, _timeUpgradeActive));
        }

        private IEnumerator DisplayImmunityUntil()
        {
            ObjectsManager.Instance.EnableDamageCollider(false);
            _immunityActivated = true;
            yield return new WaitForSeconds(_timeUpgradeActive);
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
            _uiCanvas._idleCanvas.SetActive(false);
            //TO DO: ANIMACION QUE SE DESACTIVA EL MENU;
            _isPlaying = true;
            _uiCanvas._playingCanvas.SetActive(true);
            _player.SetActive(true);
        }

        public void OpenQuitCanvas()
        {
            _uiCanvas._quitCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_yesButton);
        }

        public void QuitApplication()
        {
            PlayerPrefsManager.UpdateLevelSelected(0);
            Debug.LogError("Quit");
            Application.Quit();
        }

        public void NoButton()
        {
            _uiCanvas._quitCanvas.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_playButton);
        }

        public void ReturnMenu()
        {
            SceneManager.LoadScene("MainScene");
        } 
        #endregion
    }
}
