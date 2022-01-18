using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuView : MonoBehaviour
{
    LoadSceneParameters parameters;
    
    public void QuitButton()
    {
        Debug.LogError("Quit");
        Application.Quit();
    }

    public void LevelButton(int id)
    {
        //IMPLEMENTAR ID EN PARAMETERS
        SceneManager.LoadScene("Level"/*, parameters*/);
    }
}
