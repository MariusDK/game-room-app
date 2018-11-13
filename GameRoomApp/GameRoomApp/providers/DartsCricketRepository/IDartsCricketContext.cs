using GameRoomApp.classes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsCricketRepository
{
    public interface IDartsCricketContext
    {
        IMongoCollection<DartsCricket> dartsCricket { get; }
    }
}
