using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class JoystickManager : MonoBehaviour
    {
        [SerializeField] private GameObject _joystickButtons;

        private List<KeyCode> _keysToScan;
        private enum State { JoystickUsed, KeyboardUsed };
        private State _state;

        private void Start()
        {
            _state = State.KeyboardUsed;
            _keysToScan = new List<KeyCode>(){
                KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
                KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow,
                KeyCode.Space, KeyCode.Escape, KeyCode.Return};
        }

        private void Update()
        {
            if (_state == State.KeyboardUsed)
                ScanJoystick();
            else
                ScanKeyboard();
        }

        private void ScanJoystick()
        {
            for (int i = 0; i < 20; i++)
                if (Input.GetKeyDown("joystick button " + i))
                    SetVisible(true);
            if (Input.GetAxis("JoystickAxis") != 0) SetVisible(true);
        }

        private void ScanKeyboard()
        {
            for (int i = 0; i < _keysToScan.Count; i++)
            {
                if (Input.GetKey(_keysToScan[i]))
                    SetVisible(false);
            }
        }

        private void SetVisible(bool isJoystick)
        {
            if (isJoystick)
                _state = State.JoystickUsed;
            else
                _state = State.KeyboardUsed;
            _joystickButtons.SetActive(isJoystick);
        }
    } 
}
