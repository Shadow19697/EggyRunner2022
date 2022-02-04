using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Scripts.Managers;

public class MenuView : MonoBehaviour
{
    public TextMeshProUGUI _levelScoreText;
    public TextMeshProUGUI _settingsScoreText;
    public GameObject _mainMenuObject;
    public GameObject _levelMenuObject;
    public GameObject _settingsMenuObject;

    private void Start()
    {
        if (PlayerPrefsManager.GetLevelSelected() != 0)
        {
            _mainMenuObject.SetActive(false);
            _levelMenuObject.SetActive(true);
            PlayerPrefsManager.UpdateLevelSelected(0);

        }
    }

    [System.Obsolete]
    private void Update()
    {
        if(_levelMenuObject.active)
            _levelScoreText.text = "Score: " + PlayerPrefsManager.GetTotalScore();
        if(_settingsMenuObject.active)
            _settingsScoreText.text = "Score: " + PlayerPrefsManager.GetTotalScore();
    }

    public void QuitButton()
    {
        Debug.LogError("Quit");
        Application.Quit();
    }

    public void ToFirstLoad()
    {
        PlayerPrefsManager.ResetFirstLoad();
    }
}
