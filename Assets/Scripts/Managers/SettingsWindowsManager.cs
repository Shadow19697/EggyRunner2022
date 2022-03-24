using Scripts.Controllers.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Managers
{
    public class SettingsWindowsManager : MonoBehaviour
    {
        [SerializeField] private NavigationMenuController _navigationMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _confirmationWindow;
        [SerializeField] private GameObject _okWindow;
        [SerializeField] private List<GameObject> _saveObjects;
        [SerializeField] private List<GameObject> _eraseObjects;

        public bool SaveWindow()
        {
            bool isSave = true;
            ConfigurationWindows(isSave);
            return isSave;
        }

        public bool EraseWindow()
        {
            bool isSave = false;
            ConfigurationWindows(isSave);
            return isSave;
        }

        private void ConfigurationWindows(bool isSave)
        {
            _saveObjects.ForEach(save => save.SetActive(isSave));
            _eraseObjects.ForEach(erase => erase.SetActive(!isSave));
            _confirmationWindow.SetActive(true);
            if (isSave) {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_saveObjects[2]);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_eraseObjects[2]);
            }
        }

        public void OpenOkWindow()
        {
            _confirmationWindow.SetActive(false);
            _okWindow.SetActive(true);
            _navigationMenu.SetFirstSelectedButton(5);
        }

        public void CloseSettings()
        {
            _settingsMenu.SetActive(false);
            _mainMenu.SetActive(true);
            _confirmationWindow.SetActive(false);
            _navigationMenu.SetFirstSelectedButton(0);
        }

        public void CancelButton()
        {
            _confirmationWindow.SetActive(false);
            _navigationMenu.SetFirstSelectedButton(2);
        }

        public void OkButton(bool isSave)
        {
            _okWindow.SetActive(false);
            if (!isSave)
                _navigationMenu.SetFirstSelectedButton(2);
        }
    }
}