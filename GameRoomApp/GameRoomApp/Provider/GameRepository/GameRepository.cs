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
        public Game GetGameById(ObjectId objectId)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",objectId);
            var cursor = _gameContext.Games.Find(idFilter);
            Game game = cursor.FirstOrDefault();
            return game;
        }
        public Game GetGameByName(string name)
        {
            var builder = Builders<Game>.Filter;
            var nameFilter = builder.Eq("Name", name);
            var cursor = _gameContext.Games.Find(nameFilter);
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
        public void UpdateGameById(Game game)
        {
            FilterDefinitionBuilder<Game> GFilter = Builders<Game>.Filter;
            var UBuilder = Builders<Game>.Update;
            var idFilter = GFilter.Eq("Id", game.Id);
            var updateDefinition = UBuilder.Set("Name",game.Name).Set("Type",game.Type).Set("Players",game.Players).
                Set("StartOn",game.StartOn).Set("EndOn",game.EndOn).
                Set("EmbarrassingMoments",game.EmbarrassingMoments).Set("VictoryMoments",game.VictoryMoments);
            var cursor = _gameContext.Games.UpdateOne(idFilter,updateDefinition);
        }
        public void UpdateGameByName(string name,Game game)
        {
            FilterDefinitionBuilder<Game> GFilter = Builders<Game>.Filter;
            var UBuilder = Builders<Game>.Update;
            var nameFilter = GFilter.Eq("Name", name);
            var updateDefinition = UBuilder.Set("Name", game.Name).Set("Type", game.Type).Set("Players", game.Players).
                Set("StartOn", game.StartOn).Set("EndOn", game.EndOn).
                Set("EmbarrassingMoments", game.EmbarrassingMoments).Set("VictoryMoments", game.VictoryMoments);
            var cursor = _gameContext.Games.UpdateOne(nameFilter, updateDefinition);
        }
        public void RemoveGameById(ObjectId Id)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",Id);
            _gameContext.Games.DeleteOne(idFilter);
        }
        public void RemoveGameByName(string name)
        {
            var builder = Builders<Game>.Filter;
            var nameFilter = builder.Eq("Name", name);
            _gameContext.Games.DeleteOne(nameFilter);
        }
        public IEnumerable<Game> GetGamesByPlayer(Player player)
        {
            var builder = Builders<Game>.Filter;
            var playerFilter = builder.Eq("Players", player);
            var cursor = _gameContext.Games.Find(playerFilter);
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
        public IEnumerable<Game> GetGamesByType(string type)
        {
            var builder = Builders<Game>.Filter;
            var historyFilter = builder.Ne("EndOn", 0);
            var typeFilter = builder.Eq("Type", type);
            var filter = typeFilter & historyFilter;
            var cursor = _gameContext.Games.Find(filter);
            List<Game> games = cursor.ToList();
            return games;
        }
        public void AddPlayerToGame(string id, Player player)
        {
            ObjectId objectId = new ObjectId(id);
            Game game = GetGameById(objectId);
            List<Player> players = game.Players;
            players.Add(player);
            game.Players = players;
            UpdateGameById(game);
        }
        public void RemovePlayerFromGame(string id, string playerId)
        {
            ObjectId objectId = new ObjectId(id);
            Game game = GetGameById(objectId);
            List<Player> players = game.Players;
            foreach (Player player in players)
            {
                if (player.Id.Equals(playerId))
                {
                    players.Remove(player);
                    break;
                }
            }
            game.Players = players;
            UpdateGameById(game);
        }
    }
}
