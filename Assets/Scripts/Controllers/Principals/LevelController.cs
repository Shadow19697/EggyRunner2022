using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Scripts.Managers.InGame;
using Scripts.Enums;
using Scripts.Controllers.InGame;

namespace Scripts.Controllers.Principals
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ObjectsManager _objectsManager;
        [SerializeField] private EnvironmentController _environmentController;
        
        [SerializeField] private int _streetVelocity;
        [SerializeField] private int _velocityIncrement = 1;

        private int _counter;
        private LevelStateEnum _state;
        private int _backgroundVelocity;
        private int _gear;
        private int _intervalIncrement;

        void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            SettingsController.SetVisualSettings(false);
            _state = LevelStateEnum.IdleStart;
            _counter = 0;
            _backgroundVelocity = _streetVelocity - 5;
            _gear = 1;
            _intervalIncrement = 200;
        }

        private void FixedUpdate()
        {
            switch (_state)
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
            if (UIManager.Instance.IsPlaying())
            {
                _state = LevelStateEnum.Playing;
                _soundManager.PlayLevelMusic();
            }
        }

        private void Playing()
        {
            UpdateVelocity();
            _environmentController.MoveEnvironment(_backgroundVelocity, _streetVelocity);
            _objectsManager.UpdateVelocityMovement(_streetVelocity);
            UIManager.Instance.UpdateActualScore();
            _counter = UIManager.Instance.GetActualScore();
            if (UIManager.Instance.IsGameOver())
            {
                _state = LevelStateEnum.GameOver;
                _soundManager.PauseLevelMusic();
            }
        }

        private void UpdateVelocity()
        {
            if(_counter == (_intervalIncrement*_gear) && _gear < 7)
            {
                if (_gear == 5) _velocityIncrement--;
                _streetVelocity += _velocityIncrement;
                _gear++;
            }
        }

        private void GameOver()
        {
            _objectsManager.StopAll();
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

