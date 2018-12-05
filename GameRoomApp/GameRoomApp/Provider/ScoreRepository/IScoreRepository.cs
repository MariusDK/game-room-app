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
        void InsertScoreForAllPlayers(Game game);
        List<Score> GetScoresForGame(Game game);
        IEnumerable<Score> LeaderboardForGame(Game game);
        Score GetScoreForTeam(Team team, Game game);
        Score GetScoreById(string id);
        List<Score> LeaderboardSort(List<Score> scores);
        void GlobalLeaderboardForGameType(List<Game> games);
        void UpdateScore(Score score);
        void UpdateScoreByGame(Game newGame, Game oldGame);
        void RemoveScore(string id);
        void RemoveScoreByGame(Game game);        
    }
}
