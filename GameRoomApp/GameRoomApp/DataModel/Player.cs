using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public Player()
        { }
        public Player(string id,string Name, string Username, string Password, int Age)
        {
            this.Id = id;
            this.Name = Name;
            this.Username = Username;
            this.Password = Password;
            this.Age = Age;
        }
        public Player(string Name, string Username, string Password, int Age)
        {
            this.Name = Name;
            this.Username = Username;
            this.Password = Password;
            this.Age = Age;
        }
        public override string ToString()
        {
            return Name + " " + Age + " " ;
        }

    }
}
