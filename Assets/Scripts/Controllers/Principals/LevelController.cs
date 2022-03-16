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
        [SerializeField] private CollectableController _collectableController;

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
            
        }

        private void Playing()
        {
            _environmentController.MoveEnvironment();
            _collectableController.MoveCollectable(15);
            counter += Time.deltaTime * 8;
            _uiManager.UpdateActualScore((int)counter);
            
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

