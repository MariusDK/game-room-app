using GameRoomApp.DataModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsX01Repository
{
    public class DartsX01Context : IDartsX01Context
    {
        private readonly IMongoDatabase _database;

        public DartsX01Context(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<DartsX01> dartsX01
        {
            get
            {
                return _database.GetCollection<DartsX01>("DartsX01");
            }
        }
    }
}
