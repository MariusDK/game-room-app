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
        public string Type { get; set; }
        public List<Player> Players{ get; set; }
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }
       
    }
}
