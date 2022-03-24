using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers.Extensions
{
    public class NavigationMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _buttons;
        [SerializeField] private GameObject[] _views;

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