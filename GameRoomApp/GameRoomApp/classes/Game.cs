using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class Game
    {
        [BsonId]
        public int Id { get; set;}
        public string Name { get; set; }
        public User User { get; set; }
        public int Score { get; set; }
        public DateTime PlayedOn { get; set; }
        
    }
}
