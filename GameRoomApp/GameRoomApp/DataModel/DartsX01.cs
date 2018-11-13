using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class DartsX01
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public Score Score { get; set; }
        public int StateScore { get; set; }
    }
}
