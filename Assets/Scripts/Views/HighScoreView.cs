using Scripts.Controllers.Extensions;
using Scripts.Managers;
using Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Views
{
    public class HighScoreView : MonoBehaviour
    {
        [SerializeField] private ToggleController _localGlobalToggle;
        [SerializeField] private ToggleController _allLevelToggle;
        [SerializeField] private Dropdown _levelDropdown;
        [SerializeField] private List<RowController> _rowList;
        [SerializeField] private GameObject _errorCanvas;
        [SerializeField] private GameObject _errorCanvasButton;

        private bool isLocal;
        private bool isAll;
        private int level;
        private List<GameModel> games;

        private void Start()
        {
            VariablesInit();
            LoadTable();
        }

        private void Update()
        {
            LocalGlobalToggleSwitch();
            AllLevelToggleSwitch();
        }

        public void SetLevel(int levelIndex)
        {
            level = levelIndex + 1;
            LoadTable();
        }

        private void VariablesInit()
        {
            isLocal = _localGlobalToggle.isOn;
            isAll = _allLevelToggle.isOn;
            _levelDropdown.interactable = false;
        }

        private void LocalGlobalToggleSwitch()
        {
            if (isLocal != _localGlobalToggle.isOn)
            {
                isLocal = _localGlobalToggle.isOn;
                LoadTable();
            }
        }

        private void AllLevelToggleSwitch()
        {
            if (isAll != _allLevelToggle.isOn)
            {
                isAll = _allLevelToggle.isOn;
                if (isAll)
                {
                    _levelDropdown.interactable = false;
                    _levelDropdown.value = 0;
                }
                else _levelDropdown.interactable = true;
                level = _levelDropdown.value + 1;
                LoadTable();
            }
        }

        public void LoadTable()
        {
            _rowList.ForEach(row => row.SetLabels("-", "-", "-", "-"));
            FindHighScores();
            if (games != null)
                for (int i = 0; i < games.Count; i++)
                    _rowList[i].SetLabels((i + 1).ToString(), games[i].name, "Level " + games[i].level, games[i].score.ToString());
            else
            {
                _errorCanvas.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_errorCanvasButton);
            }
        }

        private void FindHighScores()
        {
            games = DataManager.ReturnGames(isLocal, isAll, level);
        }

        public void AddGames()
        {
            DataManager.MadeExampleGameModelList();
        }

        [System.Obsolete]
        public void ReloadView()
        {
            _localGlobalToggle.isOn = true;
            _localGlobalToggle.Toggle(true);
            _allLevelToggle.isOn = true;
            _allLevelToggle.Toggle(true);
            _levelDropdown.value = 1;
            _errorCanvas.SetActive(false);
        }
    } 
}
