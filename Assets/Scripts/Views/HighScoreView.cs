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
        [SerializeField] private GameObject _loadingCanvas;

        private bool _isLocal;
        private bool _isAll;
        private int _level;
        private List<GameModel> _games;
        private bool _fromLocalToggle;
        private bool _fromReloadButton;

        private static HighScoreView _instance;

        public static HighScoreView Instance { get { if (_instance == null) _instance = FindObjectOfType<HighScoreView>(); return _instance; } }

        private void Start()
        {
            VariablesInit();
            LoadTable(false);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                NavigationMenuController.Instance.ReturnToMainMenu();
        }

        public void LoadTable(bool fromButton)
        {
            _fromReloadButton = fromButton;
            _rowList.ForEach(row => row.SetLabels("-", "-", "-", "-"));
            if(!_isAll)
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
            _isLocal = !_localGlobalToggle.isOn;
            _isAll = !_allLevelToggle.isOn;
            _levelDropdown.interactable = false;
        }

        public void SetLevel(int levelIndex)
        {
            _level = levelIndex + 1;
            LoadTable(false);
        }

        public void SetLocalGlobal(bool isGlobal)
        {
            _isLocal = !isGlobal;
            _fromLocalToggle = true;
            LoadTable(false);
        }

        public void SetAllLevel(bool isLevel)
        {
            _isAll = !isLevel;
            if (_isAll)
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
            _games = DataManager.ReturnGames(_isLocal, _isAll, _level);
        }

        public void ReloadView()
        {
            _localGlobalToggle.isOn = false;
            _allLevelToggle.isOn = false;
            _levelDropdown.value = 0;
            _errorCanvas.SetActive(false);
        }

        public void ReloadTable()
        {
            if (!_isLocal)
            {
                _loadingCanvas.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                HttpConnectionManager.Instance.ReturnGames(true);
            }
        }

        public void HideLoadingCanvas()
        {
            _loadingCanvas.SetActive(false);
            LoadTable(true);
            SetLastButtonSelected();
        }
    } 
}
