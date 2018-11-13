using GameRoomApp.classes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsX01Repository
{
    public interface IDartsX01Context
    {
        IMongoCollection<DartsX01> dartsX01 { get; }
    }
}
