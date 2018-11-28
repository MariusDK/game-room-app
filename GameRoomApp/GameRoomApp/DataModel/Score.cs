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
       
        public Score()
        { }

        public Score(string Id, Team Team, Game Game,int Value)
        {
            this.Id = Id;
            this.Team = Team;
            this.Game = Game;
            this.Value = Value;
        }
        public Score(string Id, Team Team, Game Game)
        {
            this.Id = Id;
            this.Team = Team;
            this.Game = Game;
            this.Value = 0;
        }
        public Score(Team Team, Game Game)
        {
            this.Team = Team;
            this.Game = Game;
            this.Value = 0;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Team Team { get; set; }
        public Game Game { get; set; }
        public int Value { get; set; }
    }
}
