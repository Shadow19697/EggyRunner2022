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
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private ObstacleManager _obstacleManager;
        [SerializeField] private PerkManager _perkManager;
        [SerializeField] private EnvironmentController _environmentController;

        private float counter;
        private LevelStateEnum state;

        void Start()
        {
            SettingsController.SetVisualSettings(false);
            _soundManager.PlayLevelMusic();
            state = LevelStateEnum.IdleStart;
            counter = 0;
        }

        private void FixedUpdate()
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
            _environmentController.MoveEnvironment();
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

