using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers
{
    public class GameProvider
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Game> collection;

        public GameProvider()
        {
            client = new MongoClient();
            database = client.GetDatabase("gameRoom");
            collection = database.GetCollection<Game>("games");
        }
        public void InsertGame(Game game)
        {
            collection.InsertOne(game);
        }
        public Game GetSpecificGame(ObjectId objectId)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",objectId);
            var cursor = collection.Find(idFilter);
            Game game = cursor.FirstOrDefault();
            return game;
        }
        public List<Game> GetGames()
        {
            var builder = Builders<Game>.Filter;
            var filter = builder.Empty;
            var cursor = collection.Find(filter);
            List<Game> games = cursor.ToList();
            return games;
        }
        public void UpdateGame(Game game)
        {
            FilterDefinitionBuilder<Game> GFilter = Builders<Game>.Filter;
            var UBuilder = Builders<Game>.Update;
            var idFilter = GFilter.Eq("Id", game.Id);
            var updateDefinition = UBuilder.Set("Name",game.Name).Set("Type",game.Type).Set("Players",game.Players).
                Set("StartOn",game.StartOn).Set("EndOn",game.EndOn).
                Set("EmbarrassingMoments",game.EmbarrassingMoments).Set("VictoryMoments",game.VictoryMoments);
            var cursor = collection.UpdateOne(idFilter,updateDefinition);
        }
        public void RemoveGame(Object Id)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",Id);
            collection.DeleteOne(idFilter);
        }
        public List<Game> GetGamesThatHaveASpecificPlayer(ObjectId Id)
        {
            var builder = Builders<Game>.Filter;
            var friendFilter = builder.Eq("Player", Id);
            var cursor = collection.Find(friendFilter);
            List<Game> games = cursor.ToList();
            return games;
        }
        public List<Game> GetGameHistory()
        {
            var builder = Builders<Game>.Filter;
            var historyFilter = builder.Ne("EndOn", 0);
            var cursor = collection.Find(historyFilter);
            List<Game> games = cursor.ToList();
            return games;
        }
        public List<Game> GetGameWithType(Type type)
        {
            var builder = Builders<Game>.Filter;
            var historyFilter = builder.Ne("EndOn", 0);
            var typeFilter = builder.Eq("Type", type);
            var filter = typeFilter & historyFilter;
            var cursor = collection.Find(filter);
            List<Game> games = cursor.ToList();
            return games;
        }
    }
}
