using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class Score
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public Player Player { get; set; }
        public Game Game { get; set; }
        public int Value { get; set; } 

        public Score(Player Player, Game Game)
        {
            this.Player = Player;
            this.Game = Game;
            this.Value = 0;
        }

    }
}
