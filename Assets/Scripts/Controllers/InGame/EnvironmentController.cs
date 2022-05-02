using Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class EnvironmentController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _backgroundObjects;
        [SerializeField] private List<GameObject> _streetObjects;
        [SerializeField] private List<LevelBackgrounds> _levelsBackground;
        [System.Serializable]
        public class LevelBackgrounds
        {
            public List<Sprite> _backgrounds;
        }
        [SerializeField] private List<Sprite> _levelsStreets;

        private int _levelId;
        private List<Rigidbody2D> _rigidbodys;
        private List<SpriteRenderer> _spriteCompBg;
        private int _lastSpriteIndex = 1;
        private int _bgObjectIndex = 0;
        private bool _isBackground;


        private void Start()
        {
            _levelId = PlayerPrefsManager.GetLevelSelected()-1; //0, 1, 2, 3, 4
            GetComponents();
            SpritesInit();
            //EnvironmentInit();
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
        }

        private void SpritesInit()
        {
            for (int i = 0; i < 2; i++)
            {
                _spriteCompBg[i].sprite = _levelsBackground[_levelId]._backgrounds[i];
                if (_levelId != 2)
                    _streetObjects[i].GetComponent<SpriteRenderer>().sprite = _levelsStreets[_levelId];
                else
                {
                    _streetObjects[i].GetComponent<BoxCollider2D>().enabled = false;
                    _streetObjects[i].GetComponent<SpriteRenderer>().enabled = false;
                }
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
            if((int)list[offset].transform.localPosition.x <= -2000)
            {
                list[offset].transform.localPosition = new Vector3(
                    2050,
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
