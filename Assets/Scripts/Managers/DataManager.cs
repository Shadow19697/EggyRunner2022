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
        private static List<GameModel> _allGames;
        private static List<GameModel> _gamesToUpload = new List<GameModel>();
        private static bool wasUploaded;
        private static HttpConnectionManager _httpConnectionManager = new HttpConnectionManager();
        
        /*******************************************************************/
        public static void MadeExampleGameModelList()
        {
            for(int i=0; i<50; i++)
            {
                GameModel game = new GameModel("Nombre " + (i + 1), (i+1 * 100), i+1);
                _allGames.Add(game);
            }
            var _sortedGames = (from game in _allGames
                                orderby game.score descending
                                select game);
            LocalLoggerManager.UpdateLocalHighscoreLog(_sortedGames.ToList<GameModel>());
        }
        /*******************************************************************/

        #region Upload Games
        public static void UploadGame(string name, int score, int level)
        {
            GameModel currentGame = new GameModel(name, score, level);
            _allGames.Add(currentGame);
            var _sortedGames = (from game in _allGames
                                orderby game.score descending
                                select game);
            LocalLoggerManager.UpdateLocalHighscoreLog(_sortedGames.ToList<GameModel>());
            string gameJson = JsonConvert.SerializeObject(currentGame);
            _httpConnectionManager.PostGame(gameJson, false);
        }

        public static void AddGameToUpload(string gameJson)
        {
            GameModel game = JsonConvert.DeserializeObject<GameModel>(gameJson);
            _gamesToUpload.Add(game);
        }

        public static void ItWasUploaded(bool option)
        {
            wasUploaded = option;
        }

        public static void UploadRemainingGames()
        {
            List<GameModel> gamesUploaded = new List<GameModel>();
            _gamesToUpload.ForEach(game =>
            {
                string gameJson = JsonConvert.SerializeObject(game);
                _httpConnectionManager.PostGame(gameJson, true);
                if (wasUploaded) gamesUploaded.Add(game);
            });
            gamesUploaded.ForEach(game => _gamesToUpload.Remove(game));
        }

        #endregion

        #region Get Games
        public static List<GameModel> ReturnGames(bool isLocal, bool isAll, int level)
        {
            if (isLocal) return ReturnLocalGames(isAll, level);
            else return ReturnGlobalGames(isAll, level);
        }

        private static List<GameModel> ReturnLocalGames(bool isAll, int level)
        {
            LocalGamesInit();
            return SortGames(isAll, level, _allGames);
        }

        private static List<GameModel> ReturnGlobalGames(bool isAll, int level)
        {
            List<GameModel> recoveredGames = _httpConnectionManager.GetGlobalGames();
            if (recoveredGames != null)
            {
                Debug.Log("recuperados");
                return SortGames(isAll, level, recoveredGames);
            }
            else return null;
        }

        private static void LocalGamesInit()
        {
            _allGames = new List<GameModel>();
            try
            {
                StreamReader file = new StreamReader(LocalLoggerManager.GetLocalHighscorePath());
                var Json = file.ReadToEnd();
                file.Close();
                Debug.LogWarning(Json);
                _allGames = JsonConvert.DeserializeObject<List<GameModel>>(Json);
            }
            catch
            {
                Debug.LogWarning("No existen jugadas guardadas");
            }
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
                gamesOrdered.Add(new GameOrderedModel(0, _allGames[0]));
                gamesOrdered.Add(_allGames.Count > 1 ? new GameOrderedModel(1, _allGames[1]) : null);
                gamesOrdered.Add(_allGames.Count > 2 ? new GameOrderedModel(2, _allGames[2]) : null);
            }
            else
            {
                if(index == (_allGames.Count-1))
                {
                    gamesOrdered.Add(_allGames.Count > 2 ? new GameOrderedModel(index -2, _allGames[index - 2]) : new GameOrderedModel(index -1, _allGames[index - 1]));
                    gamesOrdered.Add(_allGames.Count > 2 ? new GameOrderedModel(index - 1, _allGames[index - 1]) : new GameOrderedModel(index, _allGames[index]));
                    gamesOrdered.Add(_allGames.Count > 2 ? new GameOrderedModel(index, _allGames[index]) : null);
                }
                else
                {
                    gamesOrdered.Add(new GameOrderedModel(index - 1, _allGames[index - 1]));
                    gamesOrdered.Add(new GameOrderedModel(index, _allGames[index]));
                    gamesOrdered.Add(new GameOrderedModel(index + 1, _allGames[index + 1]));
                }
            }
            return gamesOrdered;
        }

        public static int ReturnIndexOfGame(GameModel gameToFind)
        {
            LocalGamesInit();
            int i = 0;
            for (i = 0; i < _allGames.Count; i++)
                if (_allGames[i].level == gameToFind.level && _allGames[i].name == gameToFind.name && _allGames[i].score == gameToFind.score)
                    break;
            return i;
        }

        public static int ReturnQuantityOfGames()
        {
            return _allGames.Count;
        }
        
        #endregion

        #region Get Highscores
        public static int GetLocalHighscoreOfLevel(int level)
        {
            LocalGamesInit();
            return GetHighscoreOfLevel(level, _allGames);
        }

        public static int GetGlobalHighscoreOfLevel(int level)
        {
            List<GameModel> recoveredGames = _httpConnectionManager.GetGlobalGames();
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
