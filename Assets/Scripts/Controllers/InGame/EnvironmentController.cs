using Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class EnvironmentController : MonoBehaviour
    {
        public List<GameObject> _backgrounds;
        public List<GameObject> _streets;
        public List<Sprite> _levelsBackground;
        public List<Sprite> _levelsStreets;

        public int backgroundVelocity = 30;
        public int streetVelocity = 30;

        private int levelId;
        private List<Rigidbody2D> rigidbodys;


        private void Start()
        {
            levelId = PlayerPrefsManager.GetLevelSelected()-1; //0, 1, 2, 3, 4
            EnvironmentInit();
            GetComponents();
            //StartMoveEnvironment();
        }

        private void EnvironmentInit()
        {
            for (int i = 0; i < 2; i++)
            {
                _backgrounds[i].GetComponent<SpriteRenderer>().sprite = _levelsBackground[(levelId * 2) + i];
                if (levelId != 2)
                    _streets[i].GetComponent<SpriteRenderer>().sprite = _levelsStreets[levelId];
                else
                    _streets[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        private void GetComponents()
        {
            rigidbodys = new List<Rigidbody2D>();
            rigidbodys.Add(_backgrounds[0].GetComponent<Rigidbody2D>());
            rigidbodys.Add(_backgrounds[1].GetComponent<Rigidbody2D>());
            rigidbodys.Add(_streets[0].GetComponent<Rigidbody2D>());
            rigidbodys.Add(_streets[1].GetComponent<Rigidbody2D>());
        }

        public void StartMoveEnvironment()
        {
            for (int i = 0; i < 2; i++)
            {
                ControlMovement(i, backgroundVelocity, _backgrounds);
                if (levelId != 2)
                    ControlMovement(i + 2, streetVelocity, _streets);
            }
        }

        private void ControlMovement(int offset, int velocity, List<GameObject> list)
        {
            rigidbodys[offset].velocity = new Vector2(
                    -velocity,
                    rigidbodys[offset].velocity.y);
        }
    }
}
