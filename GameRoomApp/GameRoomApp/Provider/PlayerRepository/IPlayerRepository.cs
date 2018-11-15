﻿using GameRoomApp.DataModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.PlayerRepository
{
    public interface IPlayerRepository
    {
        void InsertPlayer(Player player);
        Player GetPlayerById(ObjectId id);
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayerByName(string name);
        void UpdatePlayer(Player player);
        void UpdatePlayerByName(string name,Player player);
        void RemovePlayer(ObjectId id);
        void RemovePlayerByName(string name);
    }
}
