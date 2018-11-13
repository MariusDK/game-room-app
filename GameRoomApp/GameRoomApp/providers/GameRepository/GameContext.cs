using System;
using System.Collections.Generic;
using System.Text;
using GameRoomApp.classes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameRoomApp.providers.GameRepository
{
    public class GameContext : IGameContext
    {
        private readonly IMongoDatabase _database;
        public GameContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<Game> Games
        {
            get
            {
                return _database.GetCollection<Game>("Games");
            }
        }
    }
}
