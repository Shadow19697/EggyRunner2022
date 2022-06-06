using Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class EnvironmentController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _backgroundObjects;
        [SerializeField] private List<GameObject> _streetObjects;
        [SerializeField] private GameObject _skyObject;
        [SerializeField] private List<LevelBackgrounds> _levelsBackground;
        [SerializeField] private List<Sprite> _skiesBackground;
        [SerializeField] private GameObject _bottomBox;
        [System.Serializable]
        public class LevelBackgrounds
        {
            public List<Sprite> _backgrounds;
        }
        [SerializeField] private List<Sprite> _levelsStreets;

        private int _levelId;
        private List<Rigidbody2D> _rigidbodys;
        private List<SpriteRenderer> _spriteCompBg;
        private SpriteRenderer _skyComponent;
        private int _lastSpriteIndex = 1;
        private int _bgObjectIndex = 0;
        private bool _isSpaceLevel;

        private static EnvironmentController _instance;

        public static EnvironmentController Instance { get { if (_instance == null) _instance = FindObjectOfType<EnvironmentController>(); return _instance; } }

        private void Start()
        {
            _levelId = PlayerPrefsManager.GetLevelSelected()-1; //0, 1, 2, 3, 4
            _isSpaceLevel = _levelId == 2;
            GetComponents();
            SpritesInit();
        }

        private void GetComponents()
        {
            _rigidbodys = new List<Rigidbody2D>();
            _rigidbodys.Add(_backgroundObjects[0].GetComponent<Rigidbody2D>());
            _rigidbodys.Add(_backgroundObjects[1].GetComponent<Rigidbody2D>());
            _rigidbodys.Add(_streetObjects[0].GetComponent<Rigidbody2D>());
            _rigidbodys.Add(_streetObjects[1].GetComponent<Rigidbody2D>());
            _spriteCompBg = new List<SpriteRenderer>();
            _spriteCompBg.Add(_backgroundObjects[0].GetComponent<SpriteRenderer>());
            _spriteCompBg.Add(_backgroundObjects[1].GetComponent<SpriteRenderer>());
            _skyComponent = _skyObject.GetComponent<SpriteRenderer>();
        }

        private void SpritesInit()
        {
            for (int i = 0; i < 2; i++)
            {
                _spriteCompBg[i].sprite = _levelsBackground[_levelId]._backgrounds[i];
                if (!_isSpaceLevel)
                    _streetObjects[i].GetComponent<SpriteRenderer>().sprite = _levelsStreets[_levelId];
                else
                    _streetObjects[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            _skyComponent.sprite = _skiesBackground[_levelId];
            if (_isSpaceLevel)
            {
                _rigidbodys[1].transform.localPosition = new Vector3(_rigidbodys[1].transform.localPosition.x+50, _rigidbodys[1].transform.localPosition.y, _rigidbodys[1].transform.localPosition.z);
                _bottomBox.transform.localPosition = new Vector3(_bottomBox.transform.localPosition.x, _bottomBox.transform.localPosition.y - 300, _bottomBox.transform.localPosition.z);
                _bottomBox.tag = "Obstacle";
            }
            
        }

        private void SetBackgroundSprite()
        {
            if (_lastSpriteIndex + 1 == _levelsBackground[_levelId]._backgrounds.Count) _lastSpriteIndex = 0;
            else _lastSpriteIndex++;
            _spriteCompBg[_bgObjectIndex].sprite = _levelsBackground[_levelId]._backgrounds[_lastSpriteIndex];
            if (_bgObjectIndex == 0) _bgObjectIndex = 1;
            else _bgObjectIndex = 0;
        }

        public void MoveEnvironment(int backgroundVelocity, int streetVelocity)
        {
            for (int i = 0; i < 2; i++)
            {
                ControlMovement(i, backgroundVelocity, _backgroundObjects);
                if (_levelId != 2)
                    ControlMovement(i + 2, streetVelocity, _streetObjects);
            }
        }

        private void ControlMovement(int offset, int velocity, List<GameObject> list)
        {
            _rigidbodys[offset].velocity = new Vector2(
                    -velocity,
                    _rigidbodys[offset].velocity.y);
            if (offset >= 2) offset -= 2;
            if((int)list[offset].transform.localPosition.x <= ((_isSpaceLevel) ? -2040 :-2000))
            {
                list[offset].transform.localPosition = new Vector3(
                    (_isSpaceLevel) ? 2150 : 2050,
                    list[offset].transform.localPosition.y,
                    list[offset].transform.localPosition.z);
                if (list[offset].transform.localPosition.y > -10) SetBackgroundSprite();
            }
        }

        public void StopEnvironment()
        {
            _rigidbodys.ForEach(rigidbody => rigidbody.velocity = new Vector2(0,0));
        }
    }
}
