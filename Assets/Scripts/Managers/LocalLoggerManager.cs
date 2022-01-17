using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public class LocalLoggerManager
    {
        private string path = Application.dataPath + "/Log.txt";
        public void CreateLocalLog()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "----------------Log----------------\n");
            }
        }

        public void EditLocalLog(int levelScore)
        {
            int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
            PlayerPrefs.SetInt("TotalScore", totalScore + levelScore);
            File.AppendAllText(path, "Score: " + levelScore + "\t- " + WorldTimeAPI.WorldTimeAPI.Instance.GetCurrentDateTime().ToString() + "\n");
        }

        public void ResetLocalLog()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                PlayerPrefs.DeleteAll();
                CreateLocalLog();
            }
        }

        public int ShowTotalScore()
        {
            return PlayerPrefs.GetInt("TotalScore", 0);
        }
    }

}
