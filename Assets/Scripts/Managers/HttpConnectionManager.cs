using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Scripts.Models;
using System;

namespace Scripts.Managers
{
    public class HttpConnectionManager : MonoBehaviour
    {
        const string _url = "";
        private List<GameModel> _globalGames;
        private Coroutine _postGameCoroutine;

        public List<GameModel> GetGlobalGames()
        {
            try
            {
                StartCoroutine(GetGamesFromAPI());
                return _globalGames;
            }
            catch
            {
                return null;
            }
        }

        IEnumerator GetGamesFromAPI()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(_url);
            yield return webRequest.SendWebRequest();
            try
            {
                _globalGames = JsonUtility.FromJson<List<GameModel>>(webRequest.downloadHandler.text);
            }
            catch (Exception)
            {
                _globalGames = null;
                Debug.LogError("No hay conexión a internet");
            }
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
