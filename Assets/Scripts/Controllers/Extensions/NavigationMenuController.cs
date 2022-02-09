using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers.Extensions
{
    public class NavigationMenuController : MonoBehaviour
    {
        public GameObject[] _buttons;
        public GameObject[] _views;

        void Start()
        {
            for(int i=0; i<_views.Length; i++)
                if (_views[i].active) SetFirstSelectedButton(i);
            //if (PlayerPrefsManager.GetLevelSelected() != 0) SetFirstSelectedButton(1);
            //else SetFirstSelectedButton(0);
        }

        public void SetFirstSelectedButton(int id)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_buttons[id]);
        }
    }

}