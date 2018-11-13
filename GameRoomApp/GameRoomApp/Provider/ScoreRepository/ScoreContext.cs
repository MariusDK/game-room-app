using System;
using System.Collections.Generic;
using System.Text;
using GameRoomApp.DataModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameRoomApp.providers.ScoreRepository
{
    public class ScoreContext : IScoreContext
    {
        public readonly IMongoDatabase _database;
        public ScoreContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<Score> Score
        {
            get
            {
                return _database.GetCollection<Score>("Score");
            }
        }
    }
}
