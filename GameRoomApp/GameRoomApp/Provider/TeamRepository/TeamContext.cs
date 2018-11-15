using GameRoomApp.DataModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.TeamRepository
{
    public class TeamContext : ITeamContext
    {
        public readonly IMongoDatabase _database;

        public TeamContext(IOptions<Settings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            _database = mongoClient.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<Team> Team
        {
            get
            {
                return _database.GetCollection<Team>("Team");
            }
        }
    }
}
