using GameRoomApp.classes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.PlayerRepository
{
    public interface IPlayerRepository
    {
        void InsertPlayer(Player player);
        Player GetPlayer(ObjectId Id);
        IEnumerable<Player> GetAllPlayers();
        void UpdatePlayer(Player player);
        void RemovePlayer(ObjectId Id);
    }
}
