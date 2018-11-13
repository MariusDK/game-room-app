using GameRoomApp.classes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.ScoreRepository
{
    public interface IScoreContext
    {
        IMongoCollection<Score> Score { get; }
    }
}
