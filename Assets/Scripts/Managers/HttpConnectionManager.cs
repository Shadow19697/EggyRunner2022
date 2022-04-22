using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Scripts.Models;
using System;
using Newtonsoft.Json;
using System.IO;
using Scripts.Views;

namespace Scripts.Managers
{
    public class HttpConnectionManager : MonoBehaviour
    {
        const string _urlPost = "https://eggyrunner.000webhostapp.com//NewScore.php";
        const string _urlGet = "https://eggyrunner.000webhostapp.com//ShowScores.php";
        private List<GameModel> _globalGames;
        private Coroutine _postGameCoroutine;
        private Coroutine _getGamesCoroutine;

        private static HttpConnectionManager _instance;

        public static HttpConnectionManager Instance { get { if (_instance == null) _instance = FindObjectOfType<HttpConnectionManager>(); return _instance; } }

        public List<GameModel> GetGlobalGames()
        {
            StreamReader file = new StreamReader(LocalLoggerManager.GetGlobalGamesPath());
            var Json = file.ReadToEnd();
            try {
                _globalGames = JsonConvert.DeserializeObject<List<GameModel>>(Json);
            } 
            catch { 
                _globalGames = null;
            }
            return _globalGames;
        }

        public void ReturnGames(bool isFromReload)
        {
            if (_getGamesCoroutine != null)
                StopCoroutine(_getGamesCoroutine);
            try
            {
                _getGamesCoroutine = StartCoroutine(GetGamesFromAPI(isFromReload));
            }
            catch
            {
                HighScoreView.Instance.HideLoadingCanvas();
                Debug.LogError("No hay conexion a internet");
            }
        }

        private IEnumerator GetGamesFromAPI(bool isFromReload)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(_urlGet);
            yield return webRequest.SendWebRequest();
            yield return new WaitForSeconds(4);
            try
            {
                Debug.Log("Se obtuvieron partidas!");
                LocalLoggerManager.UpdateGlobalGamesLog(webRequest.downloadHandler.text);
                if (isFromReload) HighScoreView.Instance.HideLoadingCanvas();
            }
            catch (Exception)
            {
                Debug.Log(webRequest.downloadHandler.text);
                Debug.LogError("No hay conexion a internet");
                if (isFromReload) HighScoreView.Instance.HideLoadingCanvas();
                LocalLoggerManager.UpdateGlobalGamesLog("");
            }
        }

        public void PostGame(GameModel game, bool isRemaining)
        {
            string gameJson = JsonConvert.SerializeObject(game);
            Debug.Log("Subiendo jugada: " + gameJson + " - Es pendiente?: " + isRemaining);
            if(_postGameCoroutine != null)
                StopCoroutine(_postGameCoroutine);
            try
            {
                _postGameCoroutine = StartCoroutine(PostGameToAPI(game, isRemaining));
            }
            catch (Exception)
            {
                Debug.LogError("No se pudo subir la jugada");
                if (!isRemaining) DataManager.AddGameToUpload(game);
            }
        }

        private IEnumerator PostGameToAPI(GameModel game, bool isRemaining) {
            var webRequest = new UnityWebRequest(_urlPost, "POST");
            string gameJson = JsonConvert.SerializeObject(game);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(gameJson);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            try
            {
                if (webRequest.downloadHandler.text != "OK") throw new Exception();
                if (isRemaining) DataManager.RemoveGameUploadedFromList(game);
                Debug.Log("Jugada subida!");
            }
            catch
            {
                Debug.LogError("No se pudo subir la jugada\n" + webRequest.error + ": " + webRequest.responseCode  + "\n" + webRequest.downloadHandler.text);
                if (!isRemaining) DataManager.AddGameToUpload(game);
            }
        }

        private void OnApplicationQuit()
        {
            if (_postGameCoroutine != null)
                StopCoroutine(_postGameCoroutine);
            if (_getGamesCoroutine != null)
                StopCoroutine(_getGamesCoroutine);
            LocalLoggerManager.ResetGlobalGamesLog();
            //PlayerPrefsManager.UpdateLevelSelected(0);
        }
    }
}
