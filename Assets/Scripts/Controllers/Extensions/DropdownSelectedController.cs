using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Controllers.Extensions
{
    public class DropdownSelectedController : MonoBehaviour, IDeselectHandler, ISelectHandler
    {
        [SerializeField] private Text _baseText;       
        private Color32 _red = new Color32(112, 0, 0, 255);
        private Color32 _champagne = new Color32(241, 223, 188, 255);

        public void OnDeselect(BaseEventData eventData)
        {
            _baseText.color = _red;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _baseText.color = _champagne;
        }
    }
} 

