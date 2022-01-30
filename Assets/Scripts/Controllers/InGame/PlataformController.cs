using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class PlataformController
    {
        public List<GameObject> _backgrounds;
        public List<GameObject> _streets;

        public void Stop()
        {
            //TO DO
            //_backgrounds.ForEach(background => background.active = false);
            //_streets.ForEach(street => street.active = false);
        }

        public void Start(int id)
        {
            //TO DO
            Stop();
            //_backgrounds[id].active = true;
            //_streets[id].active = true;
        }
    }
}
