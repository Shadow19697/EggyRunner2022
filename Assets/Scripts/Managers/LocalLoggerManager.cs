using Newtonsoft.Json;
using Scripts.Models;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public static class LocalLoggerManager
    {
        private static string _logPath = Application.dataPath + "/Log.txt";
        private static string _errorLogPath = Application.dataPath + "/ErrorLog.txt";
        private static string _playerPrefsPath = Application.dataPath + "/PlayerPrefs.txt";
        private static string _localGamesPath = Application.dataPath + "/LocalGames.txt";
        private static string _globalGamesPath = Application.dataPath + "/GlobalGames.txt";
        private static string _gamesToUploadPath = Application.dataPath + "/GamesToUpload.txt";

        #region Get
        public static string GetLogPath()
        {
            return _logPath;
        }
        public static string GetErrorLogPath()
        {
            return _errorLogPath;
        }
        public static string GetPlayerPrefsPath()
        {
            return _playerPrefsPath;
        }
        public static string GetLocalGamesPath()
        {
            return _localGamesPath;
        }
        public static string GetGlobalGamesPath()
        {
            return _globalGamesPath;
        }
        public static string GetGamesToUploadPath()
        {
            return _gamesToUploadPath;
        }
        #endregion

        #region Log
        public static void CreateLocalLog()
        {
            if (!File.Exists(_logPath))
                File.WriteAllText(_logPath, "----------------Log----------------\n");
        }
        public static void UpdateLocalLog(int levelScore)
        {
            PlayerPrefsManager.UpdateTotalScore(levelScore);
            File.AppendAllText(_logPath, "Score: " + levelScore + "\t- " + WorldTimeAPI.WorldTimeAPI.Instance.GetCurrentDateTime().ToString() + "\n");
        }
        public static void ResetLocalLog()
        {
            if (File.Exists(_logPath))
            {
                File.Delete(_logPath);
                CreateLocalLog();
            }
        }
        #endregion
        
        #region Error Log
        public static void CreateErrorLog()
        {
            if (!File.Exists(_errorLogPath))
                File.WriteAllText(_errorLogPath, "----------------Error Log----------------\n");
        }
        public static void UpdateErrorLog(string errorString)
        {
            File.AppendAllText(_errorLogPath, "Error: " + errorString + "\t- " + WorldTimeAPI.WorldTimeAPI.Instance.GetCurrentDateTime().ToString() + "\n");
        }
        public static void ResetErrorLog()
        {
            if (File.Exists(_errorLogPath))
            {
                File.Delete(_errorLogPath);
                CreateErrorLog();
            }
        }
        #endregion

        #region Player Prefs
        public static void UpdatePlayerPrefsLog(PlayerPrefsModel model)
        {
            File.Delete(_playerPrefsPath);
            string modelString = JsonConvert.SerializeObject(model);
            File.WriteAllText(_playerPrefsPath, modelString);
            //****************************************************************************
            StreamReader file = new StreamReader(_playerPrefsPath);
            var Json = file.ReadToEnd();
            Debug.Log("Se actualizó el archivo Player Pref: " + "\n" + Json);
            file.Close();
            //****************************************************************************
        }
        public static bool ExistsPlayerPrefsLog()
        {
            return File.Exists(_playerPrefsPath);
        }
        #endregion
        
        #region Local Games
        public static void UpdateLocalGamesLog(List<GameModel> games)
        {
            ResetLocalGamesLog();
            string gamesString = JsonConvert.SerializeObject(games);
            File.WriteAllText(_localGamesPath, gamesString);
        }
        public static void ResetLocalGamesLog()
        {
            if (File.Exists(_localGamesPath)) File.Delete(_localGamesPath);
        }
        #endregion

        #region Global Games
        public static void UpdateGlobalGamesLog(string games)
        {
            ResetGlobalGamesLog();
            File.WriteAllText(_globalGamesPath, games);
        }
        public static void ResetGlobalGamesLog()
        {
            if (File.Exists(_globalGamesPath)) File.Delete(_globalGamesPath); 
        }
        #endregion

        #region Games To Upload
        public static void UpdateGamesToUpload(string games)
        {
            DeleteGamesToUpload();
            File.WriteAllText(_gamesToUploadPath, games);
        }
        public static void DeleteGamesToUpload()
        {
            if (File.Exists(_gamesToUploadPath)) File.Delete(_gamesToUploadPath);
        } 
        #endregion
    }
}