using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers.Extensions
{
    public class NavigationMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _buttons;
        [SerializeField] private GameObject[] _views;

        private static NavigationMenuController _instance;

        public static NavigationMenuController Instance { get { if (_instance == null) _instance = FindObjectOfType<NavigationMenuController>(); return _instance; } }

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

        public void ReturnToMainMenu()
        {
            for (int i = 1; i < _views.Length; i++)
                _views[i].SetActive(false);
            _views[0].SetActive(true);
            SetFirstSelectedButton(0);
        }
    }
}