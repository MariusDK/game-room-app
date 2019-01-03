using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsCricketRepository
{
    public class DartsCricketRepository:IDartsCricketRepository
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
        public DartsCricket GetDartsCricket(ObjectId id)
        {
            var builder = Builders<DartsCricket>.Filter;
            var idFilter = builder.Eq("Id", id);
            var cursor = _dartsCricketContext.dartsCricket.Find(idFilter);
            DartsCricket dartsCricket = cursor.FirstOrDefault();
            return dartsCricket;
        }
        public IEnumerable<DartsCricket> GetAllDartsCricket()
        {
            var builder = Builders<DartsCricket>.Filter;
            var filter = builder.Empty;
            var cursor = _dartsCricketContext.dartsCricket.Find(filter);
            List<DartsCricket> dartsCricketList = cursor.ToList();
            return dartsCricketList;
        }
        public DartsCricket GetDartsCricketByScore(Score score)
        {
            var builder = Builders<DartsCricket>.Filter;
            var scoreFilter = builder.Eq("Score.Id", score.Id);
            var cursor = _dartsCricketContext.dartsCricket.Find(scoreFilter);
            DartsCricket dartsCricket = cursor.FirstOrDefault();
            return dartsCricket;
        }
        public void UpdateDartsCricket(DartsCricket dartsCricket)
        {
            FilterDefinitionBuilder<DartsCricket> gFilter = Builders<DartsCricket>.Filter;
            var uBuilder = Builders<DartsCricket>.Update;
            var idFilter = gFilter.Eq("Id", dartsCricket.Id);
            var updateDefinition = uBuilder.Set("Score", dartsCricket.Score).Set("Hits", dartsCricket.Hits).Set("CloseNumbers",dartsCricket.CloseNumbers).Set("OpenNumbers",dartsCricket.OpenNumbers);
            var cursor = _dartsCricketContext.dartsCricket.UpdateOne(idFilter, updateDefinition);
        }
        public void RemoveDartsCricket(string id)
        {
            var builder = Builders<DartsCricket>.Filter;
            var idFilter = builder.Eq("Id", id);
            _dartsCricketContext.dartsCricket.DeleteOne(idFilter);
        }
    }
}
