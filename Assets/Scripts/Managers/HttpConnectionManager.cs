using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;
using System.Text;
using Scripts.Models;
using System;

namespace Scripts.Managers
{
    public class HttpConnectionManager : MonoBehaviour
    {
        const string url = "";
        private List<GameModel> globalGames;

        public List<GameModel> GetGlobalGames()
        {
            StartCoroutine(GetGamesFromAPI());
            return globalGames;
        }

        IEnumerator GetGamesFromAPI()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            try
            {
                globalGames = JsonUtility.FromJson<List<GameModel>>(webRequest.downloadHandler.text);
            }
            catch (Exception)
            {
                Debug.LogError("No hay conexión a internet");
            }
        }

        public void PostGame(string json, bool isRemaining)
        {
            StartCoroutine(PostGameToAPI(json, isRemaining));
        }

        IEnumerator PostGameToAPI(string json, bool isRemaining) {
            var webRequest = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error + ": " + webRequest.responseCode);
                DataManager.ItWasUploaded(false);
                if (!isRemaining) DataManager.AddGameToUpload(json);
            }
            else
            {
                DataManager.ItWasUploaded(true);
                Debug.Log("Juego subido!");
            }
        }
    }
}
