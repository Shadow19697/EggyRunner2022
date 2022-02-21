using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Scripts.Managers.InGame;
using System;
using Scripts.Enums;
using Scripts.Controllers.InGame;

namespace Scripts.Controllers.Principals
{
    public class LevelController : MonoBehaviour
    {
        public SoundManager _soundManager;
        public PlayerController _playerController;
        public UIManager _uiManager;
        public ObstacleManager _obstacleManager;
        public PerkManager _perkManager;
        public EnvironmentController _environmentController;

        private float counter;
        private LevelStateEnum state;

        void Start()
        {
            SettingsController.SetVisualSettings(false);
            _soundManager.PlayLevelMusic();
            state = LevelStateEnum.IdleStart;
            counter = 0;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case LevelStateEnum.IdleStart: IdleStart();
                    break;
                case LevelStateEnum.Playing: Playing();
                    break;
                case LevelStateEnum.GameOver: GameOver();
                    break;
                default: break;
            }
        }

        private void IdleStart()
        {
            if (_uiManager.IsPlaying())
                state = LevelStateEnum.Playing;
            /*
            GameText.text = "Press Space to start\nPress Esc to exit";
            if (Input.GetKeyDown("space"))
            {
                Comienzo = false;
                GameText.text = "";
                Music.Play();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            //if (Input.GetKeyDown("q"))             //Lo dejo comentado para reiniciar el High Score cada vez que haga el build
            //{
            //    PlayerPrefs.DeleteAll();
            //}
            */
        }

        private void Playing()
        {
            _environmentController.StartMoveEnvironment();
            counter += Time.deltaTime * 8;
            _uiManager.UpdateActualScore((int)counter);
            /*
            counter += Time.deltaTime * 8;
            Puntaje.text = "Score: " + (int)counter;
            if ((int)counter > PlayerPrefs.GetInt("HighScore", 0)) { // Si supero el Highscore guardado
                PlayerPrefs.SetInt("HighScore", (int)counter);
                HighScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
            }
            */
        }

        private void GameOver()
        {
            /*
            if (!Fin)
            {
                Music.Stop();
                Fin = true;
                GameText.text = "Game Over\nPress R to restart\nPress Escape to exit";
                Fondo.Stop();
                File.AppendAllText(path, "High Score: " + (int)counter + "\t- " + FechaHora.text + "\n");
            }
            if (Fin)
            {
                if (Input.GetKeyDown("r"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
            */
        }
    }
}

