using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers
{
    public class MainController : MonoBehaviour
    {
        public LocalLoggerManager _logger;
        public MenuController _menu;
        public MusicManager _music;
        public DataManager _data;


        void Start()
        {
            _logger.CreateText();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
