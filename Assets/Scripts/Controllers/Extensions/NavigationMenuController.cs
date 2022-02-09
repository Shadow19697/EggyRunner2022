using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers.Extensions
{
    public class NavigationMenuController : MonoBehaviour
    {
        public GameObject[] _buttons;
        public GameObject[] _views;

        [System.Obsolete]
        void Start()
        {
            for(int i=0; i<_views.Length; i++)
                if (_views[i].active) SetFirstSelectedButton(i);
        }

        public void SetFirstSelectedButton(int id)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_buttons[id]);
        }
    }

}