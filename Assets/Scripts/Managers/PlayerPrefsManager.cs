using Newtonsoft.Json;
using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public static class PlayerPrefsManager
    {
        private static PlayerPrefsModel _model;
        private static string   FirstLoad = "FirstLoad",
                                TotalScore = "TotalScore",
                                MusicValue = "MusicValue",
                                SoundEffectsValue = "SoundEffectsValue",
                                ResolutionIndex = "ResolutionIndex",
                                QualityIndex = "QualityIndex",
                                FullScreen = "FullScreen",
                                FromPlaying = "FromPlaying",
                                LevelSelected = "LevelSelected";

        /*public static void InitPlayerPrefs()
        {
            StreamReader file = new StreamReader(Application.dataPath + "/config.txt");
            var Json = file.ReadToEnd();
            Debug.Log(Json);
            _model = JsonConvert.DeserializeObject<PlayerPrefsModel>(Json);
            file.Close();
        }*/

        #region FirstLoad Methods
        public static bool IsFirstLoad()
        {
            if (PlayerPrefs.GetInt(FirstLoad, 1) == 1) return true;
            else return false;
        }

        public static void UpdateFirstLoad()
        {
            PlayerPrefs.SetInt(FirstLoad, 0);
        } 
        #endregion

        #region TotalScore Methods
        public static void UpdateTotalScore(int levelScore)
        {
            int totalScore = PlayerPrefs.GetInt(TotalScore, 0);
            PlayerPrefs.SetInt(TotalScore, totalScore + levelScore);
        }

        public static void ResetTotalScore()
        {
            PlayerPrefs.SetInt(TotalScore, 0);
        }

        public static int GetTotalScore()
        {
            return PlayerPrefs.GetInt(TotalScore, 0);
        }
        #endregion

        #region Music & Sound Methods
        public static float GetMusicValue()
        {
            return PlayerPrefs.GetFloat(MusicValue, 1f);
        }

        public static float GetSoundEffectsValue()
        {
            return PlayerPrefs.GetFloat(SoundEffectsValue, 1f);
        }

        public static void UpdateMusicValue(float value)
        {
            PlayerPrefs.SetFloat(MusicValue, value);
        }

        public static void UpdateSoundEffectsValue(float value)
        {
            PlayerPrefs.SetFloat(SoundEffectsValue, value);
        }
        #endregion

        #region Resolution Methods
        public static int GetResolutionIndex()
        {
            return PlayerPrefs.GetInt(ResolutionIndex, 0);
        }

        public static void UpdateResolutionIndex(int value)
        {
            PlayerPrefs.SetInt(ResolutionIndex, value);
        }
        #endregion

        #region Quality Methods
        public static int GetQualityIndex()
        {
            return PlayerPrefs.GetInt(QualityIndex, 2);
        }
        
        public static void UpdateQualityIndex(int value)
        {
            PlayerPrefs.SetInt(QualityIndex, value);
        }
        #endregion

        #region FullScreen Methods
        public static bool IsFullScreen()
        {
            if (PlayerPrefs.GetInt(FullScreen, 1) == 1) return true;
            else return false;
        }

        public static void UpdateFullScreen(int value)
        {
            PlayerPrefs.SetInt(FullScreen, value);
        }
        #endregion

        #region FromPlaying Methods
        public static bool IsFromPlaying()
        {
            if (PlayerPrefs.GetInt(FromPlaying, 0) == 0) return false;
            else return true;
        }

        public static void UpdateFromPlaying()
        {
            PlayerPrefs.SetInt(FromPlaying, 1);
        }
        #endregion

        #region LevelSelected Methods
        public static int GetLevelSelected()
        {
            return PlayerPrefs.GetInt(LevelSelected, 0);
        }

        public static void UpdateLevelSelected(int level)
        {
            PlayerPrefs.SetInt(LevelSelected, level);
        } 
        #endregion
    }
}
