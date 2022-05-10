using UnityEngine;
using Scripts.Managers.InGame;
using Scripts.Enums;
using Scripts.Controllers.InGame;
using System.Collections;
using Scripts.Managers;

namespace Scripts.Controllers.Principals
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverCanvas;
        [SerializeField] private Texture2D _cursorTexture;

        [SerializeField] private int _streetVelocity;
        [SerializeField] private int _velocityIncrement;

        private Coroutine _showGameOverCanvas = null;
        private LevelStateEnum _state;
        private int _backgroundVelocity;
        private int _counter = 0;
        private int _gear = 1;
        private int _intervalIncrement = 200;

        void Start()
        {
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            LocalLoggerManager.InitLocalLoggerManager();
            SettingsController.SetVisualSettings(false);
            _gameOverCanvas.SetActive(false);
            _state = LevelStateEnum.IdleStart;
            _backgroundVelocity = _streetVelocity - 5;
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
                SoundManager.Instance.PlayLevelMusic();
            }
        }

        [System.Obsolete]
        private void Playing()
        {
            UpdateVelocity();
            EnvironmentController.Instance.MoveEnvironment(_backgroundVelocity, _streetVelocity);
            ObjectsManager.Instance.UpdateVelocityMovement(_streetVelocity);
            UIManager.Instance.UpdateActualScore();
            _counter = UIManager.Instance.GetActualScore();
            if (UIManager.Instance.IsGameOver())
            {
                _state = LevelStateEnum.GameOver;
                SoundManager.Instance.PauseLevelMusic();
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
            SoundManager.Instance.PauseLevelMusic();
            EnvironmentController.Instance.MoveEnvironment(0, 0);
            ObjectsManager.Instance.StopAll();
            if(_showGameOverCanvas == null)
                _showGameOverCanvas = StartCoroutine(ShowGameOverCanvas());
        }

        private IEnumerator ShowGameOverCanvas()
        {
            yield return new WaitForSeconds(1.5f);
            _gameOverCanvas.SetActive(true);
        }
        private void OnApplicationQuit()
        {
            if (!Application.isEditor)
                PlayerPrefsManager.UpdateLevelSelected(0);
            Debug.LogError("Quit");
        }
    }
}

