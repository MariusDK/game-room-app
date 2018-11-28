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

        public Team(string Name, List<Player> players,Game game)
        {
            this.Name = Name;
            this.Players = players;
            this.Game = game;
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public Game Game { get; set; }

        public override string ToString()
        {
            return this.Name + " " + this.Players + " "+this.Game;
        }

    }
}
