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
        [SerializeField] private Toggle _localGlobalToggle;
        [SerializeField] private Toggle _allLevelToggle;
        [SerializeField] private Dropdown _levelDropdown;
        [SerializeField] private List<RowController> _rowList;
        [SerializeField] private GameObject _errorCanvas;
        [SerializeField] private GameObject _errorCanvasButton;
        [SerializeField] private GameObject _reloadButton;

        private bool _isGlobal;
        private bool _isLevel;
        private int _level;
        private List<GameModel> _games;
        private bool _fromLocalToggle;
        private bool _fromReloadButton;

        private void Start()
        {
            VariablesInit();
            LoadTable(false);
        }

        public void LoadTable(bool fromButton)
        {
            _fromReloadButton = fromButton;
            _rowList.ForEach(row => row.SetLabels("-", "-", "-", "-"));
            if(!_isLevel)
                _levelDropdown.interactable = true;
            FindHighScores();
            if (_games != null)
                for (int i = 0; i < _games.Count; i++)
                    _rowList[i].SetLabels((i + 1).ToString(), _games[i].name, "Nivel " + _games[i].level, _games[i].score.ToString());
            else
            {
                _levelDropdown.interactable = false;
                _errorCanvas.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_errorCanvasButton);
            }
        }

        private void VariablesInit()
        {
            _isGlobal = !_localGlobalToggle.isOn;
            _isLevel = !_allLevelToggle.isOn;
            _levelDropdown.interactable = false;
        }

        public void SetLevel(int levelIndex)
        {
            _level = levelIndex + 1;
            LoadTable(false);
        }

        public void SetLocalGlobal(bool isGlobal)
        {
            _isGlobal = !isGlobal;
            _fromLocalToggle = true;
            LoadTable(false);
        }

        public void SetAllLevel(bool isLevel)
        {
            _isLevel = !isLevel;
            if (_isLevel)
            {
                _levelDropdown.interactable = false;
                _levelDropdown.value = 0;
            }
            else _levelDropdown.interactable = true;
            _level = _levelDropdown.value + 1;
            _fromLocalToggle = false;
            LoadTable(false);
        }

        public void SetLastButtonSelected()
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (_fromReloadButton)
                EventSystem.current.SetSelectedGameObject(_reloadButton);
            else
            {
                if (_fromLocalToggle)
                    EventSystem.current.SetSelectedGameObject(_localGlobalToggle.gameObject);
                else
                    EventSystem.current.SetSelectedGameObject(_allLevelToggle.gameObject);
            }
        }

        private void FindHighScores()
        {
            _games = DataManager.ReturnGames(_isGlobal, _isLevel, _level);
        }

        [System.Obsolete]
        public void ReloadView()
        {
            _localGlobalToggle.isOn = false;
            _allLevelToggle.isOn = false;
            _levelDropdown.value = 0;
            _errorCanvas.SetActive(false);
        }
    } 
}
