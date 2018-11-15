using GameRoomApp.DataModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsCricketRepository
{
    public interface IDartsCricketRepository
    {
        void InsertDartsCricket(DartsCricket dartsCricket);
        DartsCricket GetDartsCricket(ObjectId Id);
        IEnumerable<DartsCricket> GetAllDartsCricket();
        DartsCricket GetDartsCricketByScore(Score score);
        void UpdateDartsCricket(DartsCricket dartsCricket);
        void RemoveDartsCricket(string Id);
    }
}
