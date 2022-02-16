using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;
using UnityEngine.SceneManagement;

namespace Scripts.Views
{
    public class LevelView : MonoBehaviour
    {
        public List<Button> _levelButtons;
        public List<TextMeshProUGUI> _levelLabels;
        private int oldScore;

        private void Start()
        {
            UpdateButtonsStatus();
            UpdateButtonsLabels();
            oldScore = PlayerPrefsManager.GetTotalScore();
        }

        private void Update()
        {
            if(oldScore != PlayerPrefsManager.GetTotalScore())
            {
                oldScore = PlayerPrefsManager.GetTotalScore();
                UpdateButtonsStatus();
                UpdateButtonsLabels();
            }
        }

        private void UpdateButtonsStatus()
        {
            int score = PlayerPrefsManager.GetTotalScore();
            int levels = 1;
            _levelButtons.ForEach(button => button.interactable = false);
            if (score >= 1000 && score < 2000) levels = 2;
            if (score >= 2000 && score < 3000) levels = 3;
            if (score >= 3000 && score < 4000) levels = 4;
            if (score >= 4000) levels = 5;
            for (int i = 0; i < levels-1; i++)
                _levelButtons[i].interactable = true;
        }

        private void UpdateButtonsLabels()
        {
            int levelScore = 1000;
            int levelNumber = 2;
            _levelLabels.ForEach(label =>
            {
                int left = levelScore - PlayerPrefsManager.GetTotalScore();
                if (left > 0)
                    label.text = left + " LEFT TO UNLOCK";
                else
                    label.text = "LEVEL " + levelNumber;
                levelScore = levelScore + 1000;
                levelNumber++;
            });
        }

        public void LevelSelected(int id)
        {
            PlayerPrefsManager.UpdateLevelSelected(id);
            Debug.LogWarning("Nivel seleccionado: " + id);
            SceneManager.LoadScene("LevelScene");
        }
    }

}