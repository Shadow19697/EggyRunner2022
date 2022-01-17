using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public static class LocalLoggerManager
    {
        private static string path = Application.dataPath + "/Log.txt";
        public static void CreateLocalLog()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "----------------Log----------------\n");
            }
        }

        public static void EditLocalLog(int levelScore)
        {
            int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
            PlayerPrefs.SetInt("TotalScore", totalScore + levelScore);
            File.AppendAllText(path, "Score: " + levelScore + "\t- " + WorldTimeAPI.WorldTimeAPI.Instance.GetCurrentDateTime().ToString() + "\n");
        }

        public static void ResetLocalLog()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                PlayerPrefs.DeleteAll();
                CreateLocalLog();
            }
        }

        public static int ShowTotalScore()
        {
            return PlayerPrefs.GetInt("TotalScore", 0);
        }
    }

}
