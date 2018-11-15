using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class Game
    {
        //Elimina runda pentru generalizarea modelului
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Player> Players { get; set; }
        public List<Byte[]> EmbarrassingMoments { get; set; }
        public List<Byte[]> VictoryMoments { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }

        public Game()
        {
            this.StartOn = DateTime.Today;
        }
        public Game(string Id,string Name, string Type, List<Player> players)
        {
            this.Id = Id;
            this.Name = Name;
            this.Type = Type;
            this.Players = players;
            this.StartOn = DateTime.Today;
            this.EndOn = DateTime.Today;
        }
        public Game(string Name, string Type,List<Player> players)
        {
            this.Name = Name;
            this.Type = Type;
            this.Players = players;
            this.StartOn = DateTime.Today;
            this.EndOn = DateTime.Today;
        }
        public Game(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
            this.StartOn = DateTime.Today;
        }
        public Game(string Name)
        {
            this.Name = Name;
            this.StartOn = DateTime.Today;
        }

        public override string ToString()
        {
            return this.Name+" "+this.Type+" "+this.StartOn+" "+this.EndOn;
        }
    }
}
