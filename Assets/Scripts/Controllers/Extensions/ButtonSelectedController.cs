using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers.Extensions
{
    public class ButtonSelectedController : MonoBehaviour
    {
        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    } 
}
