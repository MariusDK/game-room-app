using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers
{
    public class DartsCricketProvider
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<DartsCricket> collection;

        public DartsCricketProvider()
        {
            client = new MongoClient();
            database = client.GetDatabase("gameRoom");
            collection = database.GetCollection<DartsCricket>("DartsCricket");
        }

        public void InsertDartsCricket(DartsCricket dartsCricket)
        {
            collection.InsertOne(dartsCricket);
        }
        public DartsCricket GetDartsCricket(ObjectId Id)
        {
            var builder = Builders<DartsCricket>.Filter;
            var idFilter = builder.Eq("Id", Id);
            var cursor = collection.Find(idFilter);
            DartsCricket dartsCricket = cursor.FirstOrDefault();
            return dartsCricket;
        }
        public List<DartsCricket> GetAllDartsCricket()
        {
            var builder = Builders<DartsCricket>.Filter;
            var Filter = builder.Empty;
            var cursor = collection.Find(Filter);
            List<DartsCricket> dartsCricketList = cursor.ToList();
            return dartsCricketList;
        }
        public DartsCricket GetDartsCricketByScore(Score score)
        {
            var builder = Builders<DartsCricket>.Filter;
            var scoreFilter = builder.Eq("Score", score);
            var cursor = collection.Find(scoreFilter);
            DartsCricket dartsCricket = cursor.FirstOrDefault();
            return dartsCricket;
        }
        public void UpdateDartsCricket(DartsCricket dartsCricket)
        {
            FilterDefinitionBuilder<DartsCricket> GFilter = Builders<DartsCricket>.Filter;
            var UBuilder = Builders<DartsCricket>.Update;
            var idFilter = GFilter.Eq("Id", dartsCricket.Id);
            var updateDefinition = UBuilder.Set("Score", dartsCricket.Score).Set("Hits", dartsCricket.Hits);
            var cursor = collection.UpdateOne(idFilter, updateDefinition);
        }
        public void RemoveDartsCricket(ObjectId Id)
        {
            var builder = Builders<DartsCricket>.Filter;
            var idFilter = builder.Eq("Id", Id);
            collection.DeleteOne(idFilter);
        }
    }
}
