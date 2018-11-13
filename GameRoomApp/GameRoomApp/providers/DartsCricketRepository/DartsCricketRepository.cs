using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsCricketRepository
{
    public class DartsCricketRepository
    {
        private readonly IDartsCricketContext _dartsCricketContext; 
        public DartsCricketRepository(IDartsCricketContext dartsCricketContext)
        {
            this._dartsCricketContext = dartsCricketContext;
        }

        public void InsertDartsCricket(DartsCricket dartsCricket)
        {
            _dartsCricketContext.dartsCricket.InsertOne(dartsCricket);
        }
        public DartsCricket GetDartsCricket(ObjectId Id)
        {
            var builder = Builders<DartsCricket>.Filter;
            var idFilter = builder.Eq("Id", Id);
            var cursor = _dartsCricketContext.dartsCricket.Find(idFilter);
            DartsCricket dartsCricket = cursor.FirstOrDefault();
            return dartsCricket;
        }
        public IEnumerable<DartsCricket> GetAllDartsCricket()
        {
            var builder = Builders<DartsCricket>.Filter;
            var Filter = builder.Empty;
            var cursor = _dartsCricketContext.dartsCricket.Find(Filter);
            List<DartsCricket> dartsCricketList = cursor.ToList();
            return dartsCricketList;
        }
        public DartsCricket GetDartsCricketByScore(Score score)
        {
            var builder = Builders<DartsCricket>.Filter;
            var scoreFilter = builder.Eq("Score", score);
            var cursor = _dartsCricketContext.dartsCricket.Find(scoreFilter);
            DartsCricket dartsCricket = cursor.FirstOrDefault();
            return dartsCricket;
        }
        public void UpdateDartsCricket(DartsCricket dartsCricket)
        {
            FilterDefinitionBuilder<DartsCricket> GFilter = Builders<DartsCricket>.Filter;
            var UBuilder = Builders<DartsCricket>.Update;
            var idFilter = GFilter.Eq("Id", dartsCricket.Id);
            var updateDefinition = UBuilder.Set("Score", dartsCricket.Score).Set("Hits", dartsCricket.Hits);
            var cursor = _dartsCricketContext.dartsCricket.UpdateOne(idFilter, updateDefinition);
        }
        public void RemoveDartsCricket(ObjectId Id)
        {
            var builder = Builders<DartsCricket>.Filter;
            var idFilter = builder.Eq("Id", Id);
            _dartsCricketContext.dartsCricket.DeleteOne(idFilter);
        }
    }
}
