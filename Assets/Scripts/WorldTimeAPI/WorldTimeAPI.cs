using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using Scripts.Managers;

namespace Scripts.WorldTimeAPI
{
    public class WorldTimeAPI : MonoBehaviour
    {
        const string apiUrl = "http://worldtimeapi.org/api/ip";
        private DateTime currentDateTime;
        private string errorMessage = "WorldTimeAPI - Error: No se pudo establecer conexi�n";
        struct TimeData
        {
            public string datetime;
        }

        #region Singleton class: WorldTimeAPI
        public static WorldTimeAPI Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        #endregion

        public DateTime GetCurrentDateTime()
        {
            currentDateTime = DateTime.Now;
            StartCoroutine(GetRealDateTimeFromAPI());
            return currentDateTime;
        }

        IEnumerator GetRealDateTimeFromAPI()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
            yield return webRequest.SendWebRequest();
            try
            {
                TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
                currentDateTime = ParseDateTime(timeData.datetime);
            }
            catch (Exception)
            {
                LocalLoggerManager.UpdateErrorLog(errorMessage);
                Debug.LogError(errorMessage);
            }
        }

        DateTime ParseDateTime(string datetime)
        {
            string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
            string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;
            return DateTime.Parse(string.Format("{0} {1}", date, time));
        }
    } 
}