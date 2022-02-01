using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Scripts.Managers
{
    public static class DataManager
    {
        private static List<GameModel> _allGames;

        public static void LocalGamesInit()
        {
            _allGames = new List<GameModel>();
            StreamReader file = new StreamReader(LocalLoggerManager.GetLocalHighScorePath());
            var Json = file.ReadToEnd();
            file.Close();
            _allGames = JsonConvert.DeserializeObject<List<GameModel>>(Json);
        }

        public static void MakeGameModel()
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

        public static void AddGame(string name, int score, int level)
        {
            GameModel game = new GameModel(name, score, level);
            _allGames.Add(game);
            LocalLoggerManager.UpdateLocalHighScoreLog(_allGames);
        }

        public static List<GameModel> ReturnLocalGames(bool isAll, int level)
        {
            LocalGamesInit();
            if (isAll)
            {
                var _sortedGames = (from game in _allGames
                                    orderby game.score descending
                                    select game).Take(10);
                return _sortedGames.ToList<GameModel>();
            }
            else
            {
                var _sortedGames = (from game in _allGames where game.level.Equals(level)
                                    orderby game.score descending
                                    select game).Take(10);
                return _sortedGames.ToList<GameModel>();
            }
        }

        public static List<GameModel> ReturnGames(bool isLocal, bool isAll, int level)
        {
            if (isLocal) return ReturnLocalGames(isAll, level);
            else /*TO DO*/ return ReturnLocalGames(isAll, level);
        }
    } 
}
