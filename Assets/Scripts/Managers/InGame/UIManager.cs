using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Managers.InGame
{
    public class UIManager : MonoBehaviour
    {
        public Text GameText;
        public Text Puntaje;
        public Text HighScore;
        public Text FechaHora;

        public void SetHighScore()
        {
            HighScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        }

    } 
}
