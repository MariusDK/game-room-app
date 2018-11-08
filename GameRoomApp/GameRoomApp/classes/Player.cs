using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.classes
{
    [BsonIgnoreExtraElements]
    public class Player
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public int Score { get; set; }

        public Player(string Name, string Username, string Password, int Age)
        {
            this.Name = Name;
            this.Username = Username;
            this.Password = Password;
            this.Age = Age;
        }
        public Player()
        { }
        public override string ToString()
        {
            return Name + " " + Age + " " + Score;
        }

    }
}
