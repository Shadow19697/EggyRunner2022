using Newtonsoft.Json;
using Scripts.Models;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public static class LocalLoggerManager
    {
        private static string _errorLogPath;
        private static string _playerPrefsPath;
        private static string _localGamesPath;
        private static string _globalGamesPath;
        private static string _gamesToUploadPath;
        private static string _myDocumentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/EggyRunner2022";
        private static bool _hasBeenCalled = false;

        public static void InitLocalLoggerManager()
        {
            if (!Application.isEditor)
            {
                if (!_hasBeenCalled)
                {
                    if (!File.Exists(_myDocumentsPath))
                        Directory.CreateDirectory(_myDocumentsPath);
                    _errorLogPath = _myDocumentsPath + "/ErrorLog.txt";
                    _playerPrefsPath = _myDocumentsPath + "/PlayerPrefs.txt";
                    _localGamesPath = _myDocumentsPath + "/LocalGames.txt";
                    _globalGamesPath = _myDocumentsPath + "/GlobalGames.txt";
                    _gamesToUploadPath = _myDocumentsPath + "/GamesToUpload.txt";
                    _hasBeenCalled = true;
                }
            }
            else
            {
                _errorLogPath = Application.dataPath + "/ErrorLog.txt";
                _playerPrefsPath = Application.dataPath + "/PlayerPrefs.txt";
                _localGamesPath = Application.dataPath + "/LocalGames.txt";
                _globalGamesPath = Application.dataPath + "/GlobalGames.txt";
                _gamesToUploadPath = Application.dataPath + "/GamesToUpload.txt";
            }
        }

        #region Get
        public static string GetErrorLogPath()
        {
            InitLocalLoggerManager();
            return _errorLogPath;
        }
        public static string GetPlayerPrefsPath()
        {
            InitLocalLoggerManager();
            return _playerPrefsPath;
        }
        public static string GetLocalGamesPath()
        {
            InitLocalLoggerManager();
            return _localGamesPath;
        }
        public static string GetGlobalGamesPath()
        {
            InitLocalLoggerManager();
            return _globalGamesPath;
        }
        public static string GetGamesToUploadPath()
        {
            InitLocalLoggerManager();
            return _gamesToUploadPath;
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
            InitLocalLoggerManager();
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
            InitLocalLoggerManager();
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
            InitLocalLoggerManager();
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
            InitLocalLoggerManager();
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
            InitLocalLoggerManager();
            if (File.Exists(_gamesToUploadPath)) File.Delete(_gamesToUploadPath);
        } 
        #endregion
    }
}