using GameRoomApp.classes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.TeamRepository
{
    public interface ITeamContext
    {
        IMongoCollection<Team> Team { get; }
    }
}
