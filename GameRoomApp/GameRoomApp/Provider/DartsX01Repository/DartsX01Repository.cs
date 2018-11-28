using GameRoomApp.DataModel;
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
        public DartsX01 GetDartsX01(ObjectId id)
        {
            var builder = Builders<DartsX01>.Filter;
            var idFilter = builder.Eq("Id", id);
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
            FilterDefinitionBuilder<DartsX01> gFilter = Builders<DartsX01>.Filter;
            var uBuilder = Builders<DartsX01>.Update;
            var idFilter = gFilter.Eq("Id", dartsX01.Id);
            var updateDefinition = uBuilder.Set("Score", dartsX01.Score).Set("StateScore", dartsX01.StateScore);
            var cursor = _dartsX01Context.dartsX01.UpdateOne(idFilter, updateDefinition);
        }
        public Dictionary<Team, int> LeaderboardInGameDartsX01(Game game, List<Score> scores)
        {
            var leaderboard = new Dictionary<Team, int>();
            foreach (Score score in scores)
            {
                DartsX01 dartsX01 = GetDartsX01ByScore(score);
                leaderboard.Add(score.Team,dartsX01.StateScore);
            }
            return leaderboard;
        }
        public void RemoveDartsX01(string id)
        {
            var builder = Builders<DartsX01>.Filter;
            var idFilter = builder.Eq("Id", id);
            _dartsX01Context.dartsX01.DeleteOne(idFilter);
        }
    }
}
