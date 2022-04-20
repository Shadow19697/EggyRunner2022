using Scripts.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using Scripts.Models.Nested;

namespace Scripts.Managers
{
    public static class DataManager 
    {
        private static List<GameModel> _localGames;
        private static List<GameModel> _gamesToUpload;

        #region Init
        private static void LocalGamesInit()
        {
            _localGames = new List<GameModel>();
            try
            {
                StreamReader file = new StreamReader(LocalLoggerManager.GetLocalGamesPath());
                var Json = file.ReadToEnd();
                file.Close();
                Debug.LogWarning(Json);
                _localGames = JsonConvert.DeserializeObject<List<GameModel>>(Json);
            }
            catch
            {
                Debug.LogWarning("No existen jugadas guardadas");
            }
        }

        private static void GamesToUploadInit()
        {
            _gamesToUpload = new List<GameModel>();
            try
            {
                StreamReader file = new StreamReader(LocalLoggerManager.GetGamesToUploadPath());
                var Json = file.ReadToEnd();
                file.Close();
                Debug.LogWarning(Json);
                _gamesToUpload = JsonConvert.DeserializeObject<List<GameModel>>(Json);
            }
            catch
            {
                Debug.LogWarning("No existen juegos para subir");
            }
        }
        #endregion

        #region Upload Games
        public static void UploadGame(string name, int score, int level)
        {
            LocalGamesInit();
            GameModel currentGame = new GameModel(name, score, level);
            _localGames.Add(currentGame);
            var _sortedGames = (from game in _localGames
                                orderby game.score descending
                                select game);
            LocalLoggerManager.UpdateLocalGamesLog(_sortedGames.ToList<GameModel>());
            HttpConnectionManager.Instance.PostGame(currentGame, false);
        }

        public static void AddGameToUpload(GameModel game)
        {
            GamesToUploadInit();
            _gamesToUpload.Add(game);
            LocalLoggerManager.UpdateGamesToUpload(JsonConvert.SerializeObject(_gamesToUpload));
        }

        public static void UploadRemainingGames()
        {
            GamesToUploadInit();
            List<GameModel> gamesUploaded = new List<GameModel>();
            if (_gamesToUpload.Count > 0)
                HttpConnectionManager.Instance.PostGame(_gamesToUpload[0], true);
        }

        public static void RemoveGameUploadedFromList(GameModel game)
        {
            GamesToUploadInit();
            _gamesToUpload.Remove(game);
            LocalLoggerManager.UpdateGamesToUpload(JsonConvert.SerializeObject(_gamesToUpload));
        }
        #endregion

        #region Return Games
        public static List<GameModel> ReturnGames(bool isLocal, bool isAll, int level)
        {
            if (isLocal) return ReturnLocalGames(isAll, level);
            else return ReturnGlobalGames(isAll, level);
        }

        private static List<GameModel> ReturnLocalGames(bool isAll, int level)
        {
            LocalGamesInit();
            return SortGames(isAll, level, _localGames);
        }

        private static List<GameModel> ReturnGlobalGames(bool isAll, int level)
        {
            List<GameModel> recoveredGames = HttpConnectionManager.Instance.GetGlobalGames();
            if (recoveredGames != null) return SortGames(isAll, level, recoveredGames);
            else return null;
        }

        private static List<GameModel> SortGames(bool isAll, int level, List<GameModel> games)
        {
            if (isAll)
            {
                var _sortedGames = (from game in games
                                    orderby game.score descending
                                    select game).Take(10);
                return _sortedGames.ToList<GameModel>();
            }
            else
            {
                var _sortedGames = (from game in games
                                    where game.level.Equals(level)
                                    orderby game.score descending
                                    select game).Take(10);
                return _sortedGames.ToList<GameModel>();
            }
        }

        public static List<GameOrderedModel> ReturnGamesWithLast(GameModel actualGame)
        {
            int index = ReturnIndexOfGame(actualGame);
            List<GameOrderedModel> gamesOrdered = new List<GameOrderedModel>();
            if (index == 0)
            {
                gamesOrdered.Add(new GameOrderedModel(0, _localGames[0]));
                gamesOrdered.Add(_localGames.Count > 1 ? new GameOrderedModel(1, _localGames[1]) : null);
                gamesOrdered.Add(_localGames.Count > 2 ? new GameOrderedModel(2, _localGames[2]) : null);
            }
            else
            {
                if(index == (_localGames.Count-1))
                {
                    gamesOrdered.Add(_localGames.Count > 2 ? new GameOrderedModel(index -2, _localGames[index - 2]) : new GameOrderedModel(index -1, _localGames[index - 1]));
                    gamesOrdered.Add(_localGames.Count > 2 ? new GameOrderedModel(index - 1, _localGames[index - 1]) : new GameOrderedModel(index, _localGames[index]));
                    gamesOrdered.Add(_localGames.Count > 2 ? new GameOrderedModel(index, _localGames[index]) : null);
                }
                else
                {
                    gamesOrdered.Add(new GameOrderedModel(index - 1, _localGames[index - 1]));
                    gamesOrdered.Add(new GameOrderedModel(index, _localGames[index]));
                    gamesOrdered.Add(new GameOrderedModel(index + 1, _localGames[index + 1]));
                }
            }
            return gamesOrdered;
        }

        public static int ReturnIndexOfGame(GameModel gameToFind)
        {
            LocalGamesInit();
            int i = 0;
            for (i = 0; i < _localGames.Count; i++)
                if (_localGames[i].level == gameToFind.level && _localGames[i].name == gameToFind.name && _localGames[i].score == gameToFind.score)
                    break;
            return i;
        }
        #endregion

        #region Get Highscores
        public static int GetLocalHighscoreOfLevel(int level)
        {
            LocalGamesInit();
            return GetHighscoreOfLevel(level, _localGames);
        }

        public static int GetGlobalHighscoreOfLevel(int level)
        {
            List<GameModel> recoveredGames = HttpConnectionManager.Instance.GetGlobalGames();
            if (recoveredGames != null) return GetHighscoreOfLevel(level, recoveredGames);
            else return 0;
        }

        private static int GetHighscoreOfLevel(int level, List<GameModel> games)
        {
            if (games.Count > 0)
            {
                var _game = (from game in games
                             where game.level.Equals(level)
                             orderby game.score descending
                             select game).Take(1);
                if (_game != null) return _game.ToList<GameModel>()[0].score;
                else return 0;
            }
            else return 0;
        }
        #endregion
    }
}
