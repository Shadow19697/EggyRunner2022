using Newtonsoft.Json;
using Scripts.Models;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public static class LocalLoggerManager
    {
        private static string logPath = Application.dataPath + "/Log.txt";
        private static string errorLogPath = Application.dataPath + "/ErrorLog.txt";
        private static string playerPrefsPath = Application.dataPath + "/PlayerPrefs.txt";
        private static string localHighscorePath = Application.dataPath + "/LocalHighScore.txt";

        #region Get
        public static string GetLogPath()
        {
            return logPath;
        }
        public static string GetErrorLogPath()
        {
            return errorLogPath;
        }
        public static string GetPlayerPrefsPath()
        {
            return playerPrefsPath;
        }
        public static string GetLocalHighscorePath()
        {
            return localHighscorePath;
        }
        #endregion

        #region Log
        public static void CreateLocalLog()
        {
            if (!File.Exists(logPath))
                File.WriteAllText(logPath, "----------------Log----------------\n");
        }
        public static void UpdateLocalLog(int levelScore)
        {
            PlayerPrefsManager.UpdateTotalScore(levelScore);
            File.AppendAllText(logPath, "Score: " + levelScore + "\t- " + WorldTimeAPI.WorldTimeAPI.Instance.GetCurrentDateTime().ToString() + "\n");
        }
        public static void ResetLocalLog()
        {
            if (File.Exists(logPath))
            {
                File.Delete(logPath);
                CreateLocalLog();
            }
        }
        #endregion
        
        #region Error Log
        public static void CreateErrorLog()
        {
            if (!File.Exists(errorLogPath))
                File.WriteAllText(errorLogPath, "----------------Error Log----------------\n");
        }
        public static void UpdateErrorLog(string errorString)
        {
            File.AppendAllText(errorLogPath, "Error: " + errorString + "\t- " + WorldTimeAPI.WorldTimeAPI.Instance.GetCurrentDateTime().ToString() + "\n");
        }
        public static void ResetErrorLog()
        {
            if (File.Exists(errorLogPath))
            {
                File.Delete(errorLogPath);
                CreateErrorLog();
            }
        }
        #endregion

        #region Player Prefs
        public static void UpdatePlayerPrefsLog(PlayerPrefsModel model)
        {
            File.Delete(playerPrefsPath);
            string modelString = JsonConvert.SerializeObject(model);
            File.WriteAllText(playerPrefsPath, modelString);
            //****************************************************************************
            StreamReader file = new StreamReader(playerPrefsPath);
            var Json = file.ReadToEnd();
            Debug.Log("Se actualizó el archivo Player Pref: " + "\n" + Json);
            file.Close();
            //****************************************************************************
        }
        public static bool ExistsPlayerPrefsLog()
        {
            return File.Exists(playerPrefsPath);
        }
        #endregion
        
        #region Local Highscore
        public static void UpdateLocalHighscoreLog(List<GameModel> games)
        {
            ResetLocalHighscoreLog();
            string gamesString = JsonConvert.SerializeObject(games);
            File.WriteAllText(localHighscorePath, gamesString);
        }
        public static void ResetLocalHighscoreLog()
        {
            if (File.Exists(localHighscorePath)) File.Delete(localHighscorePath);
        }
        #endregion
    }
}
