using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
        public Player GetPlayerById(ObjectId id)
        {
            var builder = Builders<Player>.Filter;
            var idFilter = builder.Eq("Id", id);
            var cursor = _playerContext.Player.Find(idFilter);
            Player player = cursor.FirstOrDefault();
            return player;
        }
        public Player login(string username,string password)
        {
            var builder = Builders<Player>.Filter;
            var usernameFilter = builder.Eq("Username",username);
            var passwordFilter = builder.Eq("Password",password);
            var filter = usernameFilter & passwordFilter;
            var cursor = _playerContext.Player.Find(filter);
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
            FilterDefinitionBuilder<Player> fBuilder = Builders<Player>.Filter;
            var uBuilder = Builders<Player>.Update;
            var idFilter = fBuilder.Eq("Id", player.Id);
            var updateDefinition = uBuilder.Set("Name", player.Name).Set("Username", player.Username).
                Set("Password", player.Password).Set("Age", player.Age);
            var cursor = _playerContext.Player.UpdateOne(idFilter, updateDefinition);
        }
        public void UpdatePlayerByName(string name, Player player)
        {
            FilterDefinitionBuilder<Player> fBuilder = Builders<Player>.Filter;
            var uBuilder = Builders<Player>.Update;
            var nameFilter = fBuilder.Eq("Name", name);
            var updateDefinition = uBuilder.Set("Name", player.Name).Set("Username", player.Username).
                Set("Password", player.Password).Set("Age", player.Age);
            var cursor = _playerContext.Player.UpdateOne(nameFilter, updateDefinition);
        }
        public void RemovePlayer(ObjectId id)
        {
            var builder = Builders<Player>.Filter;
            var idFilter = builder.Eq("Id", id);
            _playerContext.Player.DeleteOne(idFilter);
        }
        public void RemovePlayerByName(string name)
        {
            var builder = Builders<Player>.Filter;
            var nameFilter = builder.Eq("Name", name);
            _playerContext.Player.DeleteOne(nameFilter);
        }
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public Player GetPlayerByUsername(string Username)
        {
            var builder = Builders<Player>.Filter;
            var usernameFilter = builder.Eq("Username", Username);
            var cursor = _playerContext.Player.Find(usernameFilter);
            Player player = cursor.FirstOrDefault();
            return player;
        }
    }
}
