using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class DartsCricket
    {
        public DartsCricket(Score score)
        {
            this.Score = score;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Score Score { get; set; }
        public List<String> Hits { get; set; }
    }
   
}
