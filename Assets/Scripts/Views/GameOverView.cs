using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Scripts.Managers.InGame;
using Scripts.Managers;

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

        private int _score;
        private int _collectableCount;
        private int _obstacleCount;
        private int _levelScore;
        private int _increaseScore;
        private int _totalScore;

        public int CountFPS = 30;
        public float Duration = 1f;
        
        private Coroutine CountingCoroutine;

        private void Start()
        {
            _score = 1000; //UIManager.Instance.GetActualScore();
            _collectableCount = 18; //UIManager.Instance.GetEggCount();
            _obstacleCount = 5; //UIManager.Instance.GetObstaclesCount();
            _totalScore = PlayerPrefsManager.GetTotalScore();
            _increaseScore = _score + _collectableCount * 100 + _obstacleCount * 200;
            StartCoroutine(ShowCoroutine());
        }

        private IEnumerator ShowCoroutine()
        {
            SwitchResultsHighscore(true);
            _inputField.gameObject.SetActive(false);
            _continueText.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            _scoreText.gameObject.SetActive(true);
            _scoreText.text = _score.ToString();
            yield return new WaitForSeconds(2);
            _collectableText.gameObject.SetActive(true);
            _collectableText.text = _collectableCount.ToString() + " x 100";
            yield return new WaitForSeconds(2);
            _obstacleText.gameObject.SetActive(true);
            _obstacleText.text = _obstacleCount.ToString() + " x 200";
            yield return new WaitForSeconds(2);
            _levelScoreText.gameObject.SetActive(true);
            _levelScoreText.text = _levelScore.ToString();
            UpdateText(0, _increaseScore, _levelScoreText);
            yield return new WaitForSeconds(2);
            _totalScoreText.gameObject.SetActive(true);
            _totalScoreText.text = _totalScore.ToString();
            yield return new WaitForSeconds(1);
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
            int stepAmount;

            stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * Duration));

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
            Debug.LogWarning("LOL");
            //SwitchResultsHighscore(false);
        }

        private void SwitchResultsHighscore(bool isWriting)
        {
            _highscorePanel.SetActive(!isWriting);
            _resultsPanel.SetActive(isWriting);
        }
    } 
}
