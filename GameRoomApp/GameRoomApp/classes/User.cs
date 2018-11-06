using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
     
    }
}
