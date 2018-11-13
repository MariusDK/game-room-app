using System;
using System.Collections.Generic;
using System.Text;
using GameRoomApp.DataModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameRoomApp.providers.PlayerRepository
{
    public class PlayerContext : IPlayerContext
    {
        public readonly IMongoDatabase _database;
        public PlayerContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }


        public IMongoCollection<Player> Player
        {
            get
            {
                return _database.GetCollection<Player>("Player");
            }
        }
    }
}
