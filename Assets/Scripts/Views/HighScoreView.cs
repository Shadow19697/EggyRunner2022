using Scripts.Controllers;
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
    public List<TextMeshProUGUI> _positionList;
    public List<TextMeshProUGUI> _playerList;
    public List<TextMeshProUGUI> _levelList;
    public List<TextMeshProUGUI> _scoreList;

    private bool isLocal;
    private bool isAll;
    private int level;

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
        Debug.Log("Level: " + (levelIndex + 1));
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
            Debug.LogWarning("Cambió local a: " + isLocal);
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
            Debug.LogWarning("Cambió all a: " + isAll + " - Level: " + level);
        }
    }

    private void LoadTable()
    {
        for(int i = 0; i < 10; i++)
        {
            _positionList[i].text = (i + 1).ToString();
        }
    }
}
