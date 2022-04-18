using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Scripts.Models;
using System;
using Newtonsoft.Json;
using System.IO;

namespace Scripts.Managers
{
    public class HttpConnectionManager : MonoBehaviour
    {
        const string _url = "";
        const string _urlGet = "https://eggyrunner.000webhostapp.com//ShowScores.php";
        private List<GameModel> _globalGames;
        private Coroutine _postGameCoroutine;
        private Coroutine _getGamesCoroutine;
        private bool _isReady = false;

        private static HttpConnectionManager _instance;

        public static HttpConnectionManager Instance { get { if (_instance == null) _instance = FindObjectOfType<HttpConnectionManager>(); return _instance; } }

        public List<GameModel> GetGlobalGames()
        {
            StreamReader file = new StreamReader(Application.dataPath + "/RecoveredGames.txt");
            var Json = file.ReadToEnd();
            return JsonConvert.DeserializeObject<List<GameModel>>(Json);
            //try
            //{
            //    Debug.Log("1");
            //    if (_getGamesCoroutine == null)
            //    {
            //        Debug.Log("2");
            //        _getGamesCoroutine = StartCoroutine(GetGamesFromAPI());
            //    }
            //    Debug.Log("3");
            //    while (_getGamesCoroutine != null)
            //    {
            //        Debug.Log("Obteniendo partidas");
            //    }
            //    Debug.Log("4");
            //    return _globalGames;
            //}
            //catch
            //{
            //    Debug.Log("5");
            //    return null;
            //}
        }

        public bool IsReady()
        {
            return _isReady;
        }

        public void ReturnGames()
        {
            _isReady = false;
            _getGamesCoroutine = StartCoroutine(GetGamesFromAPI());
        }

        private IEnumerator GetGamesFromAPI()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(_urlGet);
            yield return webRequest.SendWebRequest();
            try
            {
                Debug.Log(webRequest.downloadHandler.text);
                _globalGames = JsonConvert.DeserializeObject<List<GameModel>>(webRequest.downloadHandler.text);
                string gamesString = JsonConvert.SerializeObject(_globalGames);
                File.WriteAllText(Application.dataPath + "/RecoveredGames.txt", gamesString);
            }
            catch (Exception)
            {
                _globalGames = null;
                Debug.LogError("No hay conexion a internet");
            }
            _isReady = true;
        }

        public void PostGame(string json, bool isRemaining)
        {
            Debug.Log("json: " + json + " remaining: " + isRemaining);
            if(_postGameCoroutine != null)
                StopCoroutine(_postGameCoroutine);
            try
            {
                _postGameCoroutine = StartCoroutine(PostGameToAPI(json, isRemaining));
            }
            catch (Exception)
            {
                Debug.LogWarning("No se pudo subir la jugada");
                DataManager.ItWasUploaded(false);
                if (!isRemaining) DataManager.AddGameToUpload(json);
            }
        }

        IEnumerator PostGameToAPI(string json, bool isRemaining) {
            var webRequest = new UnityWebRequest(_url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            try
            {
                DataManager.ItWasUploaded(true);
                Debug.Log("Juego subido!");
            }
            catch
            {
                Debug.Log(webRequest.error + ": " + webRequest.responseCode);
                DataManager.ItWasUploaded(false);
                if (!isRemaining) DataManager.AddGameToUpload(json);
            }
            //if (webRequest.result != UnityWebRequest.Result.Success)
            //{
            //    Debug.Log(webRequest.error + ": " + webRequest.responseCode);
            //    DataManager.ItWasUploaded(false);
            //    if (!isRemaining) DataManager.AddGameToUpload(json);
            //}
            //else
            //{
            //    DataManager.ItWasUploaded(true);
            //    Debug.Log("Juego subido!");
            //}
        }
    }
}
