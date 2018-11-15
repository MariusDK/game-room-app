using GameRoomApp.DataModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.GameRepository
{
    public interface IGameRepository
    {
        void InsertGame(Game game);
        Game GetGameById(ObjectId objectId);
        Game GetGameByName(string name);
        IEnumerable<Game> GetAllGames();
        IEnumerable<Game> GetGamesByPlayer(Player player);
        IEnumerable<Game> GetGameHistory();
        IEnumerable<Game> GetGamesByType(string type);
        void UpdateGameById(Game game);
        void UpdateGameByName(string name,Game game);
        void RemoveGameById(ObjectId Id);
        void RemoveGameByName(string Id);
        void AddPlayerToGame(string id, Player player);
        void RemovePlayerFromGame(string id, string playerId);
    }
}

