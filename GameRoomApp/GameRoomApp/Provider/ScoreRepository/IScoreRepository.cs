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
        IEnumerable<Score> GetScoresForGame(Game game);
        IEnumerable<Score> LeaderboardForGame(Game game);
        Score GetScoreForPlayer(Player player, Game game);
        void LeaderboardSort(List<Score> scores);
        void GlobalLeaderboardForGameType(List<Game> games);
        void UpdateScore(Score score);
        void RemoveScore(ObjectId Id);
    }
}
