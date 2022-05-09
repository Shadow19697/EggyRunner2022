using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Scripts.Managers.InGame;
using Scripts.Managers;
using Scripts.Controllers.Extensions;
using Scripts.Models;
using Scripts.Models.Nested;
using UnityEngine.SceneManagement;

namespace Scripts.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Panels _panels;
        [System.Serializable]
        public class Panels
        {
            public GameObject _resultsPanel;
            public GameObject _highscorePanel;
            public GameObject _dontSavePanel;
            public GameObject _cantSavePanel;
        }
        
        [SerializeField] private TMPObjects _tmpObjects;
        [System.Serializable]
        public class TMPObjects
        {
            public TextMeshProUGUI _resultsText;
            public TextMeshProUGUI _scoreText;
            public TextMeshProUGUI _collectableText;
            public TextMeshProUGUI _obstacleText;
            public TextMeshProUGUI _levelScoreText;
            public TextMeshProUGUI _totalScoreText;
            public TextMeshProUGUI _skipText;
        }

        [SerializeField] private ButtonsObjects _buttonsObjects;
        [System.Serializable]
        public class ButtonsObjects
        {
            public GameObject _return;
            public GameObject _replay;
            public GameObject _yes;
            public GameObject _accept;
        }

        [SerializeField] private InputField _inputField;
        [SerializeField] private GameObject _continueText;
        [SerializeField] private Text _nameText;
        [SerializeField] private List<RowController> _rowList;

        private int _score;
        private int _collectableCount;
        private int _obstacleCount;
        private int _levelScore;
        private int _increaseScore;
        private int _totalScore;
        private int _countFPS = 30;
        private float _duration = 1f;
        private Coroutine _countingCoroutine;
        private Coroutine _showCoroutine;
        private bool _skip = false;
        private bool _isSpaceLevel;

        private void Start()
        {
            SetInitValues();
            if (_isSpaceLevel) _tmpObjects._resultsText.text = "PUNTAJE\nNANOSATELITES RECUPERADOS\n\nPUNTAJE DE LA PARTIDA\n\nPUNTOS ACUMULADOS";
            _tmpObjects._skipText.gameObject.SetActive(true);
            _tmpObjects._skipText.text = "Presiones [ESPACIO] para saltar conteo";
            _showCoroutine = StartCoroutine(ShowCoroutine(1));
        }

        private void SetInitValues()
        {
            _score = UIManager.Instance.GetActualScore();
            _collectableCount = UIManager.Instance.GetEggCount();
            _obstacleCount = UIManager.Instance.GetObstaclesCount();
            _totalScore = PlayerPrefsManager.GetTotalScore();
            _increaseScore = _score + _collectableCount * 200 + _obstacleCount * 50;
            _isSpaceLevel = PlayerPrefsManager.GetLevelSelected() == 3;
        }

        [System.Obsolete]
        private void Update()
        {
            if (!_panels._highscorePanel.active) { 
                if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) && !_skip)
                {
                    _skip = true;
                    StopAllCoroutines();
                    _showCoroutine = StartCoroutine(ShowCoroutine(0));
                }
                if (Input.GetButtonDown("Cancel") && !_panels._dontSavePanel.active)
                    CancelInput();
                else if ((Input.GetButtonDown("Cancel") && _panels._dontSavePanel.active))
                    NoButton();
            }
            else
            {
                if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) && _skip)
                {
                    _skip = false;
                    StopAllCoroutines();
                    StartCoroutine(ShowHighscores(0));
                }
            }
        }

        #region Show Results Methods
        private IEnumerator ShowCoroutine(float waitTime)
        {
            SwitchResultsHighscore(true);
            yield return new WaitForSeconds(waitTime);
            _tmpObjects._scoreText.gameObject.SetActive(true);
            _tmpObjects._scoreText.text = _score.ToString();
            yield return new WaitForSeconds(waitTime);
            _tmpObjects._collectableText.gameObject.SetActive(true);
            _tmpObjects._collectableText.text = _collectableCount.ToString() + " x 200";
            if (!_isSpaceLevel)
            {
                yield return new WaitForSeconds(waitTime);
                _tmpObjects._obstacleText.gameObject.SetActive(true);
                _tmpObjects._obstacleText.text = _obstacleCount.ToString() + " x 50";
            }
            yield return new WaitForSeconds(waitTime);
            _tmpObjects._levelScoreText.gameObject.SetActive(true);
            _tmpObjects._levelScoreText.text = _levelScore.ToString();
            if (waitTime > 0) UpdateText(0, _increaseScore, _tmpObjects._levelScoreText);
            else _tmpObjects._levelScoreText.text = _increaseScore.ToString();
            yield return new WaitForSeconds(waitTime);
            _tmpObjects._totalScoreText.gameObject.SetActive(true);
            _tmpObjects._totalScoreText.text = _totalScore.ToString();
            yield return new WaitForSeconds(waitTime);
            if (waitTime > 0) UpdateText(_totalScore, _totalScore + _increaseScore, _tmpObjects._totalScoreText);
            else _tmpObjects._totalScoreText.text = (_totalScore + _increaseScore).ToString();
            yield return new WaitForSeconds(waitTime * 1.5f);
            _inputField.gameObject.SetActive(true);
            _continueText.SetActive(true);
            _tmpObjects._skipText.gameObject.SetActive(false);
            SetSelectedButton(_inputField.gameObject);
        }

        private void UpdateText(int previousValue, int newValue, TextMeshProUGUI text)
        {
            if (_countingCoroutine != null)
                StopCoroutine(_countingCoroutine);
            _countingCoroutine = StartCoroutine(CountText(previousValue, newValue, text));
        }

        private IEnumerator CountText(int previousValue, int newValue, TextMeshProUGUI text)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / _countFPS);
            int stepAmount = Mathf.CeilToInt((newValue - previousValue) / (_countFPS * _duration));
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                    previousValue = newValue;
                text.SetText(previousValue.ToString());
                yield return Wait;
            }
        }
        #endregion

        #region Display Highscores Methods
        private void DisplayHighscores(bool gameWasSaved)
        {
            SetHighscores(gameWasSaved);
            _rowList.ForEach(row => row.gameObject.SetActive(false));
            StartCoroutine(ShowHighscores(0.5f));
            _tmpObjects._skipText.gameObject.SetActive(true);
        }

        private void SetHighscores(bool gameWasSaved)
        {
            _rowList.ForEach(row => row.SetLabels("-", "-", "-", "-"));
            List<GameOrderedModel> games = new List<GameOrderedModel>();
            if (gameWasSaved)
            {
                GameModel currentGame = new GameModel(_nameText.text, _increaseScore, PlayerPrefsManager.GetLevelSelected());
                games = DataManager.ReturnGamesWithLast(currentGame);
            }
            else
            {
                List<GameModel> top3games = DataManager.ReturnGames(true, false, PlayerPrefsManager.GetLevelSelected());
                for (int i=0; i<3; i++)
                    games.Add(top3games.Count > i ? new GameOrderedModel((i), top3games[i]) : null);
            }
            for (int i = 0; i < games.Count; i++)
                if (games[i] != null)
                    _rowList[i].SetLabels((games[i].position + 1).ToString(), games[i].game.name, "Nivel " + games[i].game.level, games[i].game.score.ToString());
        }

        private IEnumerator ShowHighscores(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _rowList[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            _rowList[1].gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            _rowList[2].gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            _buttonsObjects._replay.SetActive(true);
            _buttonsObjects._return.SetActive(true);
            _tmpObjects._skipText.gameObject.SetActive(false);
            SetSelectedButton(_buttonsObjects._return);
        } 
        #endregion

        private void SwitchResultsHighscore(bool isWriting)
        {
            _panels._highscorePanel.SetActive(!isWriting);
            _panels._resultsPanel.SetActive(isWriting);
        }

        #region Button Methods
        public void AcceptInput()
        {
            if (!string.IsNullOrWhiteSpace(_nameText.text) && !string.IsNullOrEmpty(_nameText.text))
            {
                PlayerPrefsManager.UpdateTotalScore(_increaseScore);
                DataManager.UploadGame(_nameText.text, _increaseScore, PlayerPrefsManager.GetLevelSelected());
                SwitchResultsHighscore(false);
                DisplayHighscores(true);
            }
            else
            {
                _panels._cantSavePanel.SetActive(true);
                SetSelectedButton(_buttonsObjects._accept);
            }
        }

        public void CancelInput()
        {
            _panels._dontSavePanel.SetActive(true);
            SetSelectedButton(_buttonsObjects._yes);
        }

        public void AcceptButton()
        {
            _panels._cantSavePanel.SetActive(false);
            SetSelectedButton(_inputField.gameObject);
        }

        public void YesButton()
        {
            SwitchResultsHighscore(false);
            DisplayHighscores(false);
        }

        public void NoButton()
        {
            _panels._dontSavePanel.SetActive(false);
            SetSelectedButton(_inputField.gameObject);
        }

        public void ReloadButton()
        {
            SceneManager.LoadScene("LevelScene");
        }

        public void ReturnToMenuButton()
        {
            SceneManager.LoadScene("MainScene");
        }        
        #endregion

        private void SetSelectedButton(GameObject button)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    } 
}
