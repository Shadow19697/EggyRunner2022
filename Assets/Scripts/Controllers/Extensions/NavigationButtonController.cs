using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Controllers.Extensions
{
    [RequireComponent(typeof(Selectable))]
    public class NavigationButtonController : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!EventSystem.current.alreadySelecting)
            {
                if (this.gameObject.GetComponent<Button>() != null || this.gameObject.GetComponent<Dropdown>() != null) { 
                    if (this.gameObject.GetComponent<Button>() != null && this.gameObject.GetComponent<Button>().interactable)
                        EventSystem.current.SetSelectedGameObject(this.gameObject);
                    if (this.gameObject.GetComponent<Dropdown>() != null && this.gameObject.GetComponent<Dropdown>().interactable)
                        EventSystem.current.SetSelectedGameObject(this.gameObject);
                }
                else
                    EventSystem.current.SetSelectedGameObject(this.gameObject);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            this.GetComponent<Selectable>().OnPointerExit(null);
        }
    } 
}