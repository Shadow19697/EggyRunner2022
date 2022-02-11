using Scripts.Managers;
using System.Collections;
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

        private List<Sprite> backgroundsSelected;
        private List<Sprite> streetsSelected;

        private void Start()
        {
            BackgroundInit();
            StreetInit();
        }

        private void BackgroundInit()
        {
            
        }

        private void StreetInit()
        {
            
        }
    }
}
