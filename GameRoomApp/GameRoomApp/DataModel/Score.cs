using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class Score
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Player Player { get; set; }
        public Game Game { get; set; }
        public int Value { get; set; }

        public Score()
        { }

        public Score(string Id, Player Player, Game Game,int Value)
        {
            this.Id = Id;
            this.Player = Player;
            this.Game = Game;
            this.Value = Value;
        }
        public Score(string Id, Player Player, Game Game)
        {
            this.Id = Id;
            this.Player = Player;
            this.Game = Game;
            this.Value = 0;
        }
        public Score(Player Player, Game Game)
        {
            this.Player = Player;
            this.Game = Game;
            this.Value = 0;
        }
    }
}
