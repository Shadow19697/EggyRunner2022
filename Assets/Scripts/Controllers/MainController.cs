using Scripts.Enums;
using Scripts.Managers;
using Scripts.WorldTimeAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers
{
    public class MainController : MonoBehaviour
    {
        public MenuController _menu;
        public SoundManager _sound;
        //public DataManager _data;
        
        private SpecialDateEnum specialEnum;

        void Start()
        {
            LocalLoggerManager.CreateLocalLog();
            specialEnum = SpecialDate.WichSpecialIs();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
