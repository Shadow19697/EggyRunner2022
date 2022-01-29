using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Managers
{
    public class LevelManager : MonoBehaviour
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
            for (int i = 0; i < levels; i++)
                _levelButtons[i].interactable = true;
        }

        private void UpdateButtonsLabels()
        {
            int necesario = 1000;
            int levelNumber = 2;
            _levelLabels.ForEach(label =>
            {
                int left = necesario - PlayerPrefsManager.GetTotalScore();
                if (left > 0)
                    label.text = left + " LEFT TO UNLOCK";
                else
                    label.text = "LEVEL " + levelNumber;
                necesario = necesario + 1000;
                levelNumber++;
            });
        }

    }

}