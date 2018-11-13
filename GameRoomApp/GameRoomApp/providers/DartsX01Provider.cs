using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers
{
    public class DartsX01Provider
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<DartsX01> collection;

        public DartsX01Provider()
        {
            client = new MongoClient();
            database = client.GetDatabase("gameRoom");
            collection = database.GetCollection<DartsX01>("DartsX01");
        }
        public void InsertDartsX01(DartsX01 dartsX01)
        {
            collection.InsertOne(dartsX01);
        }
        public DartsX01 GetDartsX01(ObjectId Id)
        {
            var builder = Builders<DartsX01>.Filter;
            var idFilter = builder.Eq("Id", Id);
            var cursor = collection.Find(idFilter);
            DartsX01 dartsX01 = cursor.FirstOrDefault();
            return dartsX01;
        }
        public List<DartsX01> GetAllDartsX01()
        {
            var builder = Builders<DartsX01>.Filter;
            var filter = builder.Empty;
            var cursor = collection.Find(filter);
            List<DartsX01> dartsX01List = cursor.ToList();
            return dartsX01List;
        }
        public DartsX01 GetDartsX01ByScore(Score score)
        {
            var builder = Builders<DartsX01>.Filter;
            var scoreFilter = builder.Eq("Score", score);
            var cursor = collection.Find(scoreFilter);
            DartsX01 dartsX01 = cursor.FirstOrDefault();
            return dartsX01;
        }
        public void UpdateDartsX01(DartsX01 dartsX01)
        {
            FilterDefinitionBuilder<DartsX01> GFilter = Builders<DartsX01>.Filter;
            var UBuilder = Builders<DartsX01>.Update;
            var idFilter = GFilter.Eq("Id", dartsX01.Id);
            var updateDefinition = UBuilder.Set("Score", dartsX01.Score).Set("StateScore", dartsX01.StateScore);
            var cursor = collection.UpdateOne(idFilter, updateDefinition);
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
            //Sortare dictionar
        }
    }
}
