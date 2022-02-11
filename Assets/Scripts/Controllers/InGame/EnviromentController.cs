using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class EnviromentController : MonoBehaviour
    {
        public List<GameObject> _backgrounds;
        public List<GameObject> _streets;

        private GameObject backgroundSelected;
        private GameObject streetSelected;

        private void Start()
        {
            BackgroundInit();
            StreetInit();
        }

        private void BackgroundInit()
        {
            _backgrounds.ForEach(background => background.SetActive(false));
            backgroundSelected = _backgrounds[(PlayerPrefsManager.GetLevelSelected() - 1)];
            backgroundSelected.SetActive(true);
        }

        private void StreetInit()
        {
            _streets.ForEach(street => street.SetActive(false));
            streetSelected = _streets[(PlayerPrefsManager.GetLevelSelected() - 1)];
            streetSelected.SetActive(true);
        }
    }
}
