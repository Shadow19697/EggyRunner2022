using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Controllers.InGame
{
    public class EnvironmentController : MonoBehaviour
    {
        public List<GameObject> _backgrounds;
        public List<GameObject> _streets;
        public List<Sprite> _levelsBackground;
        public List<Sprite> _levelsStreets;

        private int levelId;

        private SpriteRenderer spriteRenderer;
        public Sprite newSprite;

        private void Start()
        {
            levelId = PlayerPrefsManager.GetLevelSelected()-1; //0, 1, 2, 3, 4
            BackgroundInit();
            StreetInit();
        }

        private void Update()
        {
            StartMoveBackground();
        }

        private void BackgroundInit()
        {
            for (int i = 0; i < 2; i++)
                _backgrounds[i].GetComponent<Image>().sprite = _levelsBackground[(levelId * 2)+i];
        }

        public void StartMoveBackground()
        {
            for (int i = 0; i < 2; i++)
            {
                _backgrounds[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-250,
                    _backgrounds[i].GetComponent<Rigidbody2D>().velocity.y);
                Debug.Log(_backgrounds[0].transform.localPosition.x);
                if ((int)_backgrounds[i].transform.localPosition.x <= -1950)
                {
                    _backgrounds[i].transform.localPosition = new Vector3(
                        1950,
                        _backgrounds[i].transform.localPosition.y,
                        _backgrounds[i].transform.localPosition.z);
                }
            }
        }

        private void StreetInit()
        {
            
        }

        /**********************************************************************************************************/
        public void ReturnMenu()
        {
            SceneManager.LoadScene("MainScene");
        }
        /**********************************************************************************************************/
    }
}
