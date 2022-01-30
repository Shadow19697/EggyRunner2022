using Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreView : MonoBehaviour
{
    public ToggleController _localGlobalToggle;
    public ToggleController _allLevelToggle;
    public Dropdown _levelDropdown;

    private bool isLocal;
    private bool isAll;
    private int level;

    private void Start()
    {
        isLocal = _localGlobalToggle.isOn;
        isAll = _allLevelToggle.isOn;
        _levelDropdown.interactable = false;
    }

    private void Update()
    {
        if (isLocal != _localGlobalToggle.isOn)
        {
            isLocal = _localGlobalToggle.isOn;
            Debug.LogWarning("Cambió local a: " + isLocal);
        }
        if (isAll != _allLevelToggle.isOn)
        {
            isAll = _allLevelToggle.isOn;
            if (isAll)
            {
                _levelDropdown.interactable = false;
                _levelDropdown.value = 0;
            }
            else _levelDropdown.interactable = true;
            level= _levelDropdown.value+1;
            Debug.LogWarning("Cambió all a: " + isAll + " - Level: " + level);
        }
    }

    public void SetLevel(int levelIndex)
    {
        level = levelIndex+1;
        Debug.Log("Level: " + (levelIndex + 1));
    }
}
