using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public class LocalLoggerManager
    {
        public void CreateText()
        {
            string path = Application.dataPath + "/Records.txt";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "----------------Records----------------\n");
            }
        }
    }

}
