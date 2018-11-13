using GameRoomApp.classes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.GameRepository
{
    public interface IGameContext
    {
        IMongoCollection<Game> Games { get; }
    }
}
