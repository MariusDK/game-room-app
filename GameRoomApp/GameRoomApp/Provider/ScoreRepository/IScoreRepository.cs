using GameRoomApp.DataModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.ScoreRepository
{
    public interface IScoreRepository
    {
        void InsertScore(Score score);
        void InsertScoreForAllPlayer(Game game);
        IEnumerable<Score> GetScoresForGame(Game game);
        IEnumerable<Score> LeaderboardForGame(Game game);
        Score GetScoreForPlayer(Player player, Game game);
        Score GetScoreById(string id);
        void LeaderboardSort(List<Score> scores);
        void GlobalLeaderboardForGameType(List<Game> games);
        void UpdateScore(Score score);
        void UpdateScoreByGame(Game newGame, Game oldGame);
        void RemoveScore(string Id);
        void RemoveScoreByGame(Game game);
        
    }
}
