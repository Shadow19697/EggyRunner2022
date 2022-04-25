using Newtonsoft.Json;
using Scripts.Models;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Managers
{
    public static class PlayerPrefsManager
    {
        private static PlayerPrefsModel _model = new PlayerPrefsModel();
        private static string FirstLoad = "firstLoad",
                                TotalScore = "totalScore",
                                LevelSelected = "levelSelected",
                                ResolutionIndex = "resolutionIndex",
                                Height = "height",
                                Width = "width",
                                FullScreen = "fullScreen",
                                MusicValue = "musicValue",
                                SoundEffectsValue = "soundEffectsValue";

        public static Resolution[] Resolutions;
        public static List<string> ListResolutions = new List<string>();

        public static void InitPlayerPrefs()
        {
            if (LocalLoggerManager.ExistsPlayerPrefsLog())
            {
                ReadModel();
            }
            else
            {
                _model.firstLoad = GetFirstLoad();
                _model.totalScore = GetTotalScore();
                _model.levelSelected = GetLevelSelected();
                _model.settings = new SettingsModel();
                _model.settings.resolutionIndex = GetResolutionIndex();
                _model.settings.height = GetHeight();
                _model.settings.width = GetWidth();
                _model.settings.fullScreen = GetFullScreen();
                _model.settings.musicValue = GetMusicValue();
                _model.settings.soundEffectsValue = GetSoundEffectsValue();
                /*******************************************************/
                Debug.Log("Se creó archivo player prefs");
                /*******************************************************/
                LocalLoggerManager.UpdatePlayerPrefsLog(_model);
            }
        }

        private static void ReadModel()
        {
            StreamReader file = new StreamReader(LocalLoggerManager.GetPlayerPrefsPath());
            var Json = file.ReadToEnd();
            _model = JsonConvert.DeserializeObject<PlayerPrefsModel>(Json);
            file.Close();
            /*******************************************************/
            Debug.Log("Se leyó el archivo player prefs" + Json);
            /*******************************************************/
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
            ReadModel();
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
            
        }

        public static void UpdateWidth(int value)
        {
            PlayerPrefs.SetInt(Width, value);
            
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
        }

        public static int GetFullScreen()
        {
            return PlayerPrefs.GetInt(FullScreen, 1);
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

        public static void UpdateSettingsValues(SettingsModel _newSettings)
        {
            _model.settings.fullScreen = _newSettings.fullScreen;
            _model.settings.height = _newSettings.height;
            _model.settings.musicValue = _newSettings.musicValue;
            _model.settings.resolutionIndex = _newSettings.resolutionIndex;
            _model.settings.soundEffectsValue = _newSettings.soundEffectsValue;
            _model.settings.width = _newSettings.width;
            UpdateFullScreen(_newSettings.fullScreen);
            UpdateHeight(_newSettings.height);
            UpdateMusicValue(_newSettings.musicValue);
            UpdateResolutionIndex(_newSettings.resolutionIndex);
            UpdateSoundEffectsValue(_newSettings.soundEffectsValue);
            UpdateWidth(_newSettings.width);
            LocalLoggerManager.UpdatePlayerPrefsLog(_model);
        }

        public static SettingsModel GetSettingValues()
        {
            return _model.settings;
        }
    }
}
