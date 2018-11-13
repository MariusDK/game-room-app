using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class Game
    {
        //Elimina runda pentru generalizarea modelului
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Player> Players { get; set; }
        public List<Byte[]> EmbarrassingMoments { get; set; }
        public List<Byte[]> VictoryMoments { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }


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
