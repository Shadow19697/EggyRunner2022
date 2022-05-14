using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;
using UnityEngine.SceneManagement;
using Scripts.Controllers.Extensions;

namespace Scripts.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private List<Button> _levelButtons;
        [SerializeField] private List<TextMeshProUGUI> _levelLabels;

        private int oldScore;
        private int _pointsForUnlock = 10000;

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
            if (Input.GetButtonDown("Cancel"))
                NavigationMenuController.Instance.ReturnToMainMenu();
        }

        private void UpdateButtonsStatus()
        {
            int score = PlayerPrefsManager.GetTotalScore();
            int levels = 1;
            _levelButtons.ForEach(button => button.interactable = false);
            if (score >= _pointsForUnlock) levels = 2;
            /*if (score >= _pointsForUnlock*2 && score < _pointsForUnlock*3) levels = 3;
            if (score >= _pointsForUnlock*3 && score < _pointsForUnlock*4) levels = 4;
            if (score >= _pointsForUnlock*4) levels = 5;
            */
            for (int i = 0; i < levels-1; i++)
                _levelButtons[i].interactable = true;
        }

        private void UpdateButtonsLabels()
        {
            int levelScore = _pointsForUnlock;
            int levelNumber = 2;
            _levelLabels.ForEach(label =>
            {
                int left = levelScore - PlayerPrefsManager.GetTotalScore();
                if (left > 0)
                    label.text = "FALTAN " + left + " PUNTOS";
                else
                    label.text = "NIVEL " + levelNumber;
                levelScore = levelScore + _pointsForUnlock;
                levelNumber++;
            });
            //-----------------------------ERASE--------------------------
            for(int i = 1; i<4; i++)
                _levelLabels[i].text = "BLOQUEADO";
            //------------------------------------------------------------
        }

        public void LevelSelected(int id)
        {
            PlayerPrefsManager.UpdateLevelSelected(id);
            Debug.LogWarning("Nivel seleccionado: " + id);
            SceneManager.LoadScene("LevelScene");
        }
    }

}