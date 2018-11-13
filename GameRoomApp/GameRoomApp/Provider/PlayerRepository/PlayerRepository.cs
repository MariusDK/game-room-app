using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
namespace GameRoomApp.providers.PlayerRepository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IPlayerContext _playerContext;

        public PlayerRepository(IPlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public void InsertPlayer(Player player)
        {
            _playerContext.Player.InsertOne(player);
        }
        public Player GetPlayerByName(string name)
        {
            var builder = Builders<Player>.Filter;
            var nameFilter = builder.Eq("Name", name);
            var cursor = _playerContext.Player.Find(nameFilter);
            Player player = cursor.FirstOrDefault();
            return player;
        }
        public Player GetPlayer(ObjectId Id)
        {
            var builder = Builders<Player>.Filter;
            var idFilter = builder.Eq("Id", Id);
            var cursor = _playerContext.Player.Find(idFilter);
            Player player = cursor.FirstOrDefault();
            return player;
        }
        public IEnumerable<Player> GetAllPlayers()
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.Empty;
            var cursor = _playerContext.Player.Find(filter);
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
            var cursor = _playerContext.Player.UpdateOne(idFilter, updateDefinition);
        }
        public void UpdatePlayerByName(string name, Player player)
        {
            FilterDefinitionBuilder<Player> Fbuilder = Builders<Player>.Filter;
            var UBuilder = Builders<Player>.Update;
            var nameFilter = Fbuilder.Eq("Name", name);
            var updateDefinition = UBuilder.Set("Name", player.Name).Set("Username", player.Username).
                Set("Password", player.Password).Set("Age", player.Age);
            var cursor = _playerContext.Player.UpdateOne(nameFilter, updateDefinition);
        }
        public void RemovePlayer(ObjectId Id)
        {
            var builder = Builders<Player>.Filter;
            var idFilter = builder.Eq("Id", Id);
            _playerContext.Player.DeleteOne(idFilter);
        }
        public void RemovePlayerByName(string name)
        {
            var builder = Builders<Player>.Filter;
            var nameFilter = builder.Eq("Name", name);
            _playerContext.Player.DeleteOne(nameFilter);
        }


    }
}
