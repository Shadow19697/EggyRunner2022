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
        private static PlayerPrefsModel _model = new PlayerPrefsModel();
        private static string   FirstLoad = "firstLoad",
                                TotalScore = "totalScore",
                                MusicValue = "musicValue",
                                SoundEffectsValue = "soundEffectsValue",
                                ResolutionIndex = "resolutionIndex",
                                QualityIndex = "qualityIndex",
                                FullScreen = "fullScreen",
                                LevelSelected = "levelSelected",
                                Height = "height",
                                Width = "width";

        public static Resolution[] Resolutions;
        public static List<string> ListResolutions = new List<string>();
        
        public static void InitPlayerPrefs()
        {
            if (LocalLoggerManager.ExistsPlayerPrefsLog())
            {
                StreamReader file = new StreamReader(LocalLoggerManager.GetPlayerPrefsPath());
                var Json = file.ReadToEnd();
                Debug.Log(Json);
                _model = JsonConvert.DeserializeObject<PlayerPrefsModel>(Json);
                file.Close();
                Debug.Log("Se leyó el archivo player prefs");
            }
            else
            {
                _model.firstLoad = GetFirstLoad();
                _model.totalScore = GetTotalScore();
                _model.musicValue = GetMusicValue();
                _model.soundEffectsValue = GetSoundEffectsValue();
                _model.resolutionIndex = GetResolutionIndex();
                _model.qualityIndex = GetQualityIndex();
                _model.fullScreen = GetFullScreen();
                _model.levelSelected = GetLevelSelected();
                _model.height = GetHeight();
                _model.width = GetWidth();
                Debug.Log("Se creó archivo player prefs");
                LocalLoggerManager.UpdatePlayerPrefsLog(_model);
            }
        }
        
        #region FirstLoad Methods
        public static bool IsFirstLoad()
        {
            if (GetFirstLoad() == 1) return true;
            else return false;
        }

        public static void UpdateFirstLoad()
        {
            PlayerPrefs.SetInt(FirstLoad, 0);
            _model.firstLoad = 0;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }

        public static void ResetFirstLoad()
        {
            PlayerPrefs.SetInt(FirstLoad, 1);
            _model.firstLoad = 1;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }
        public static int GetFirstLoad()
        {
            return PlayerPrefs.GetInt(FirstLoad, 1);
        }
        #endregion

        #region TotalScore Methods
        public static void UpdateTotalScore(int levelScore)
        {
            int totalScore = PlayerPrefs.GetInt(TotalScore, 0);
            PlayerPrefs.SetInt(TotalScore, totalScore + levelScore);
            _model.totalScore = totalScore + levelScore;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }

        public static void ResetTotalScore()
        {
            PlayerPrefs.SetInt(TotalScore, 0);
            _model.totalScore = 0;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
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
            _model.musicValue = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }

        public static void UpdateSoundEffectsValue(float value)
        {
            PlayerPrefs.SetFloat(SoundEffectsValue, value);
            _model.soundEffectsValue = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
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
            _model.resolutionIndex = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
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
            _model.qualityIndex = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }
        #endregion

        #region FullScreen Methods
        public static bool IsFullScreen()
        {
            if (GetFullScreen() == 1) return true;
            else return false;
        }

        public static void UpdateFullScreen(int value)
        {
            PlayerPrefs.SetInt(FullScreen, value);
            _model.fullScreen = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }

        public static int GetFullScreen()
        {
            return PlayerPrefs.GetInt(FullScreen, 1);
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
            _model.levelSelected = level;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }
        #endregion

        #region Height & Width Methods
        public static int GetHeight()
        {
            return PlayerPrefs.GetInt(Height, 0);
        }

        public static int GetWidth()
        {
            return PlayerPrefs.GetInt(Width, 0);
        }

        public static void UpdateHeight(int value)
        {
            PlayerPrefs.SetInt(Height, value);
            _model.height = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }

        public static void UpdateWidth(int value)
        {
            PlayerPrefs.SetInt(Width, value);
            _model.width = value;
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }
        #endregion

    }
}
