using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class Player
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public List<Game> History { get; set; }
        public int Score { get; set; }

    }
}
