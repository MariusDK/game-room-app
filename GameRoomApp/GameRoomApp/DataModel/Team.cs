using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class Team
    {
        public Team()
        { }
        public Team(string Name, List<Player> Players)
        {
            this.Name = Name;
            this.Players = Players;
        }
        public Team(string id,string name, List<Player> players)
        {
            this.Id = Id;
            this.Name = name;
            this.Players = players;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }

        public override string ToString()
        {
            return this.Name + " " + this.Players;
        }

    }
}
