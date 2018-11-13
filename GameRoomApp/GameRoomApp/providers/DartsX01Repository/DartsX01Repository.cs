using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsX01Repository
{
    public class DartsX01Repository:IDartsX01Repository
    {
        private readonly IDartsX01Context _dartsX01Context;

        public DartsX01Repository(IDartsX01Context dartsX01Context)
        {
            this._dartsX01Context = dartsX01Context;
        }
        public void InsertDartsX01(DartsX01 dartsX01)
        {
            _dartsX01Context.dartsX01.InsertOne(dartsX01);
        }
        public DartsX01 GetDartsX01(ObjectId Id)
        {
            var builder = Builders<DartsX01>.Filter;
            var idFilter = builder.Eq("Id", Id);
            var cursor = _dartsX01Context.dartsX01.Find(idFilter);
            DartsX01 dartsX01 = cursor.FirstOrDefault();
            return dartsX01;
        }
        public IEnumerable<DartsX01> GetAllDartsX01()
        {
            var builder = Builders<DartsX01>.Filter;
            var filter = builder.Empty;
            var cursor = _dartsX01Context.dartsX01.Find(filter);
            List<DartsX01> dartsX01List = cursor.ToList();
            return dartsX01List;
        }
        public DartsX01 GetDartsX01ByScore(Score score)
        {
            var builder = Builders<DartsX01>.Filter;
            var scoreFilter = builder.Eq("Score", score);
            var cursor = _dartsX01Context.dartsX01.Find(scoreFilter);
            DartsX01 dartsX01 = cursor.FirstOrDefault();
            return dartsX01;
        }
        public void UpdateDartsX01(DartsX01 dartsX01)
        {
            FilterDefinitionBuilder<DartsX01> GFilter = Builders<DartsX01>.Filter;
            var UBuilder = Builders<DartsX01>.Update;
            var idFilter = GFilter.Eq("Id", dartsX01.Id);
            var updateDefinition = UBuilder.Set("Score", dartsX01.Score).Set("StateScore", dartsX01.StateScore);
            var cursor = _dartsX01Context.dartsX01.UpdateOne(idFilter, updateDefinition);
        }
        public Dictionary<Player, int> LeaderboardInGameDartsX01(Game game, List<Score> scores)
        {
            var leaderboard = new Dictionary<Player, int>();
            foreach (Score score in scores)
            {
                DartsX01 dartsX01 = GetDartsX01ByScore(score);
                leaderboard.Add(score.Player,dartsX01.StateScore);
            }
            return leaderboard;
        }
    }
}
