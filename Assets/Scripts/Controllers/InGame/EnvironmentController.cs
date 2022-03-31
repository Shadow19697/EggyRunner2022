using Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class EnvironmentController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _backgrounds;
        [SerializeField] private List<GameObject> _streets;
        [SerializeField] private List<Sprite> _levelsBackground;
        [SerializeField] private List<Sprite> _levelsStreets;

        private int _levelId;
        private List<Rigidbody2D> _rigidbodys;


        private void Start()
        {
            _levelId = PlayerPrefsManager.GetLevelSelected()-1; //0, 1, 2, 3, 4
            EnvironmentInit();
            GetComponents();
        }

        private void EnvironmentInit()
        {
            for (int i = 0; i < 2; i++)
            {
                _backgrounds[i].GetComponent<SpriteRenderer>().sprite = _levelsBackground[(_levelId * 2) + i];
                if (_levelId != 2)
                    _streets[i].GetComponent<SpriteRenderer>().sprite = _levelsStreets[_levelId];
                else
                {
                    _streets[i].GetComponent<BoxCollider2D>().enabled = false;
                    _streets[i].GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        private void GetComponents()
        {
            _rigidbodys = new List<Rigidbody2D>();
            _rigidbodys.Add(_backgrounds[0].GetComponent<Rigidbody2D>());
            _rigidbodys.Add(_backgrounds[1].GetComponent<Rigidbody2D>());
            _rigidbodys.Add(_streets[0].GetComponent<Rigidbody2D>());
            _rigidbodys.Add(_streets[1].GetComponent<Rigidbody2D>());
        }

        public void MoveEnvironment(int backgroundVelocity, int streetVelocity)
        {
            for (int i = 0; i < 2; i++)
            {
                ControlMovement(i, backgroundVelocity, _backgrounds);
                if (_levelId != 2)
                    ControlMovement(i + 2, streetVelocity, _streets);
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
            }
        }

        public void StopEnvironment()
        {
            _rigidbodys.ForEach(rigidbody => rigidbody.velocity = new Vector2(0,0));
        }
    }
}
