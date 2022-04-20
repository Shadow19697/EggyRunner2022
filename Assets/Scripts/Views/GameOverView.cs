using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Scripts.Managers.InGame;
using Scripts.Managers;
using System;
using Scripts.Controllers.Extensions;
using Scripts.Models;
using Scripts.Models.Nested;
using UnityEngine.SceneManagement;

namespace Scripts.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject _resultsPanel;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _collectableText;
        [SerializeField] private TextMeshProUGUI _obstacleText;
        [SerializeField] private TextMeshProUGUI _levelScoreText;
        [SerializeField] private TextMeshProUGUI _totalScoreText;
        [SerializeField] private InputField _inputField;
        [SerializeField] private GameObject _continueText;
        [SerializeField] private Text _nameText;

        [SerializeField] private GameObject _highscorePanel;
        [SerializeField] private List<RowController> _rowList;
        [SerializeField] private List<GameObject> _buttons;

        private int _score;
        private int _collectableCount;
        private int _obstacleCount;
        private int _levelScore;
        private int _increaseScore;
        private int _totalScore;

        private int CountFPS = 30;
        private float Duration = 1f;
        
        private Coroutine CountingCoroutine;

        private void Start()
        {
            _score = 1400; //UIManager.Instance.GetActualScore();
            _collectableCount = 20; //UIManager.Instance.GetEggCount();
            _obstacleCount = 7; //UIManager.Instance.GetObstaclesCount();
            _totalScore = PlayerPrefsManager.GetTotalScore();
            _increaseScore = _score + _collectableCount * 100 + _obstacleCount * 200;
            StartCoroutine(ShowCoroutine());
        }

        private IEnumerator ShowCoroutine()
        {
            SwitchResultsHighscore(true);
            yield return new WaitForSeconds(1);
            _scoreText.gameObject.SetActive(true);
            _scoreText.text = _score.ToString();
            yield return new WaitForSeconds(1);
            _collectableText.gameObject.SetActive(true);
            _collectableText.text = _collectableCount.ToString() + " x 100";
            yield return new WaitForSeconds(1);
            _obstacleText.gameObject.SetActive(true);
            _obstacleText.text = _obstacleCount.ToString() + " x 200";
            yield return new WaitForSeconds(1);
            _levelScoreText.gameObject.SetActive(true);
            _levelScoreText.text = _levelScore.ToString();
            UpdateText(0, _increaseScore, _levelScoreText);
            yield return new WaitForSeconds(1f);
            _totalScoreText.gameObject.SetActive(true);
            _totalScoreText.text = _totalScore.ToString();
            yield return new WaitForSeconds(1.5f);
            UpdateText(_totalScore, _totalScore + _increaseScore, _totalScoreText);
            yield return new WaitForSeconds(2);
            _inputField.gameObject.SetActive(true);
            _continueText.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
        }

        private void UpdateText(int previousValue, int newValue, TextMeshProUGUI text)
        {
            if (CountingCoroutine != null)
                StopCoroutine(CountingCoroutine);
            CountingCoroutine = StartCoroutine(CountText(previousValue, newValue, text));
        }

        private IEnumerator CountText(int previousValue, int newValue, TextMeshProUGUI text)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
            int stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * Duration));
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                    previousValue = newValue;
                text.SetText(previousValue.ToString());
                yield return Wait;
            }
        }

        public void AcceptButton()
        {
            PlayerPrefsManager.UpdateTotalScore(_increaseScore);
            DataManager.UploadGame(_nameText.text, _increaseScore, PlayerPrefsManager.GetLevelSelected());
            SwitchResultsHighscore(false);
            DisplayHighscores();
        }

        private void DisplayHighscores()
        {
            SetHighscores();
            _rowList.ForEach(row => row.gameObject.SetActive(false));
            StartCoroutine(ShowHighscores());
        }

        private IEnumerator ShowHighscores()
        {
            yield return new WaitForSeconds(1);
            _rowList[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _rowList[1].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _rowList[2].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _buttons.ForEach(button => button.gameObject.SetActive(true));
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
        }

        private void SwitchResultsHighscore(bool isWriting)
        {
            _highscorePanel.SetActive(!isWriting);
            _resultsPanel.SetActive(isWriting);
        }

        private void SetHighscores()
        {
            _rowList.ForEach(row => row.SetLabels("-", "-", "-", "-"));
            GameModel currentGame = new GameModel(_nameText.text, _increaseScore, PlayerPrefsManager.GetLevelSelected());
            List<GameOrderedModel> games = DataManager.ReturnGamesWithLast(currentGame);
            for (int i = 0; i < games.Count; i++)
                if(games[i] != null)
                    _rowList[i].SetLabels((games[i].position+1).ToString(), games[i].game.name, "Nivel " + games[i].game.level, games[i].game.score.ToString());
        }

        public void ReloadButton()
        {
            SceneManager.LoadScene("LevelScene");
        }

        public void ReturnToMenuButton()
        {
            SceneManager.LoadScene("MainScene");
        }
    } 
}
