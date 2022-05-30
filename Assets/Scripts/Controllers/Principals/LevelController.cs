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
        [SerializeField] private GameObject _enviromentController;
        [SerializeField] private GameObject _objectsManager;

        [SerializeField] private int _streetVelocity;
        [SerializeField] private int _velocityIncrement;

        private Coroutine _showGameOverCanvas = null;
        private LevelStateEnum _state;
        private int _backgroundVelocity;
        private int _gear = 1;
        private int _intervalIncrement = 22;
        private float _timePlaying = 0;
        private bool _isSpace;

        void Start()
        {
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            LocalLoggerManager.InitLocalLoggerManager();
            _gameOverCanvas.SetActive(false);
            _state = LevelStateEnum.Cinematic;
            _isSpace = PlayerPrefsManager.GetLevelSelected() == 3;
            if (_isSpace) _streetVelocity = _streetVelocity - 4;
            _backgroundVelocity = _streetVelocity - 5;
            _enviromentController.SetActive(false);
            _objectsManager.SetActive(false);
            SoundManager.Instance.PlayCinematicMusic();
        }

        [System.Obsolete]
        private void FixedUpdate()
        {
            switch (_state)
            {
                case LevelStateEnum.Cinematic: Cinematic();
                    break;
                case LevelStateEnum.IdleStart: IdleStart();
                    break;
                case LevelStateEnum.Playing: Playing();
                    break;
                case LevelStateEnum.GameOver: GameOver();
                    break;
                default: break;
            }
        }

        private void Cinematic()
        {
            if (!UIManager.Instance.IsCinematicPlaying())
            {
                _state = LevelStateEnum.IdleStart;
                SoundManager.Instance.StopCinematicMusic();
                _enviromentController.SetActive(true);
                _objectsManager.SetActive(true);
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
            _timePlaying += Time.deltaTime;
            if (UIManager.Instance.IsGameOver())
            {
                _state = LevelStateEnum.GameOver;
                UIManager.Instance.SetWhiteScoreText();
                SoundManager.Instance.PauseLevelMusic();
            }
        }

        private void UpdateVelocity()
        {
            if((int)_timePlaying == (_intervalIncrement*_gear) && _gear < 7)
            {
                Debug.LogWarning("Increment " + _gear);
                if (_gear == 5) _velocityIncrement--;
                _streetVelocity += _velocityIncrement;
                _gear++;
            }
        }

        private void GameOver()
        {
            SoundManager.Instance.PauseMusic();
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

