using GameRoomApp.classes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.PlayerRepository
{
    public interface IPlayerContext
    {
        IMongoCollection<Player> Player { get; }
    }
}
