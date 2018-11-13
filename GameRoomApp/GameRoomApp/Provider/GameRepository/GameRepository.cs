using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.GameRepository
{
    public class GameRepository : IGameRepository
    {
        private readonly IGameContext _gameContext;

        public GameRepository(IGameContext gameContext)
        {
            _gameContext = gameContext;
        }
        public void InsertGame(Game game)
        {
            _gameContext.Games.InsertOne(game);
        }
        public Game GetSpecificGame(ObjectId objectId)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",objectId);
            var cursor = _gameContext.Games.Find(idFilter);
            Game game = cursor.FirstOrDefault();
            return game;
        }
        public IEnumerable<Game> GetAllGames()
        {
            var builder = Builders<Game>.Filter;
            var filter = builder.Empty;
            var cursor = _gameContext.Games.Find(filter);
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
            var cursor = _gameContext.Games.UpdateOne(idFilter,updateDefinition);
        }
        public void RemoveGame(ObjectId Id)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",Id);
            _gameContext.Games.DeleteOne(idFilter);
        }
        public IEnumerable<Game> GetGamesByPlayer(ObjectId Id)
        {
            var builder = Builders<Game>.Filter;
            var friendFilter = builder.Eq("Player", Id);
            var cursor = _gameContext.Games.Find(friendFilter);
            List<Game> games = cursor.ToList();
            return games;
        }
        public IEnumerable<Game> GetGameHistory()
        {
            var builder = Builders<Game>.Filter;
            var historyFilter = builder.Ne("EndOn", 0);
            var cursor = _gameContext.Games.Find(historyFilter);
            List<Game> games = cursor.ToList();
            return games;
        }
        public IEnumerable<Game> GetGameWithType(Type type)
        {
            var builder = Builders<Game>.Filter;
            var historyFilter = builder.Ne("EndOn", 0);
            var typeFilter = builder.Eq("Type", type);
            var filter = typeFilter & historyFilter;
            var cursor = _gameContext.Games.Find(filter);
            List<Game> games = cursor.ToList();
            return games;
        }
    }
}
