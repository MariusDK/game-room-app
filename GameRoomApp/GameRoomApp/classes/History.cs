using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class History
    {
        [BsonId]
        public int Id { get; set;}
        public List<Game> Games { get; set; }

    }
}
