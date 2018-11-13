using System;
using System.Collections.Generic;
using System.Text;
using GameRoomApp.classes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameRoomApp.providers.DartsCricketRepository
{
    public class DartsCricketContext : IDartsCricketContext
    {
        private readonly IMongoDatabase _database;
        public DartsCricketContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<DartsCricket> dartsCricket
        {
            get {
                return _database.GetCollection<DartsCricket>("DartsCricket");
            }
        }
    }
}
