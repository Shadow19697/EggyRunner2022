using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scripts.Managers
{
    public static class DataManager 
    {
        private static List<GameModel> _allGames;
        private static List<GameModel> _gamesToUpload = new List<GameModel>();
        private static bool wasUploaded;
        private static HttpConnectionManager _httpConnectionManager = new HttpConnectionManager();
        
        
        public static void MadeExampleGameModelList()
        {
            //_allGames = new List<GameModel>();
            for(int i=0; i<50; i++)
            {
                GameModel game = new GameModel("Nombre " + (i + 1), (i+1 * 100), i+1);
                _allGames.Add(game);
            }
            var _sortedGames = (from game in _allGames
                                orderby game.score descending
                                select game);
            LocalLoggerManager.UpdateLocalHighScoreLog(_sortedGames.ToList<GameModel>());
        }


        public static void UploadGame(string name, int score, int level)
        {
            GameModel game = new GameModel(name, score, level);
            _allGames.Add(game);
            LocalLoggerManager.UpdateLocalHighScoreLog(_allGames);
            string gameJson = JsonConvert.SerializeObject(game);
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
                if(wasUploaded) gamesUploaded.Add(game);
            });
            gamesUploaded.ForEach(game => _gamesToUpload.Remove(game));
        }

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

        private static void LocalGamesInit()
        {
            _allGames = new List<GameModel>();
            StreamReader file = new StreamReader(LocalLoggerManager.GetLocalHighScorePath());
            var Json = file.ReadToEnd();
            file.Close();
            _allGames = JsonConvert.DeserializeObject<List<GameModel>>(Json);
        }

        private static List<GameModel> ReturnGlobalGames (bool isAll, int level)
        {
            List<GameModel> recoveredGames = _httpConnectionManager.GetGlobalGames();
            if (recoveredGames != null) return SortGames(isAll, level, recoveredGames);
            else return null;
        }

        private static List<GameModel> SortGames (bool isAll, int level, List<GameModel> games)
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
    } 
}
