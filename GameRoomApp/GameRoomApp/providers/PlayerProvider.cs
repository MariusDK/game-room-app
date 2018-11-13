using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
namespace GameRoomApp.providers
{
    public class PlayerProvider
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Player> collection;

        public PlayerProvider()
        {
            client = new MongoClient();
            database = client.GetDatabase("gameRoom");
            collection = database.GetCollection<Player>("players");
        }

        public void InsertPlayer(Player player)
        {
            collection.InsertOne(player);
        }
        public Player GetSpecificPlayer(ObjectId Id)
        {
            var builder = Builders<Player>.Filter;
            var idFilter = builder.Eq("Id", Id);
            var cursor = collection.Find(idFilter);
            Player player = cursor.FirstOrDefault();
            return player;
        }
        public List<Player> GetPlayers()
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.Empty;
            var cursor = collection.Find(filter);
            List<Player> players = cursor.ToList();
            return players;
        }
        public void UpdatePlayer(Player player)
        {
            FilterDefinitionBuilder<Player> Fbuilder = Builders<Player>.Filter;
            var UBuilder = Builders<Player>.Update;
            var idFilter = Fbuilder.Eq("Id", player.Id);
            var updateDefinition = UBuilder.Set("Name", player.Name).Set("Username", player.Username).
                Set("Password", player.Password).Set("Age", player.Age);
            var cursor = collection.UpdateOne(idFilter, updateDefinition);
        }
        public void RemovePlayer(ObjectId Id)
        {
            var builder = Builders<Player>.Filter;
            var idFilter = builder.Eq("Id", Id);
            collection.DeleteOne(idFilter);
        }

    }
}
