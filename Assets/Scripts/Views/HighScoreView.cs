using Scripts.Controllers;
using Scripts.Managers;
using Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreView : MonoBehaviour
{
    public ToggleController _localGlobalToggle;
    public ToggleController _allLevelToggle;
    public Dropdown _levelDropdown;
    public List<RowController> _rowList;

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
        level = levelIndex+1;
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

    private void LoadTable()
    {
        FindHighScores();
        _rowList.ForEach(row => row.SetLabels("-", "-", "-", "-"));
        for (int i = 0; i < games.Count; i++)
            _rowList[i].SetLabels((i + 1).ToString(), games[i].name, "Level " + games[i].level, games[i].score.ToString());
    }

    private void FindHighScores()
    {
        games = DataManager.ReturnGames(isLocal, isAll, level);
    }
}
