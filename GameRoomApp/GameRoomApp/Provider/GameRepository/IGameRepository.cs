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
        List<Game> GetGamesByTeam(Team team);
        IEnumerable<Game> GetGameHistory();
        IEnumerable<Game> GetGamesByType(string type);
        void UpdateGameById(Game game);
        void UpdateGameByName(string name,Game game);
        void RemoveGameById(ObjectId id);
        void RemoveGameByName(string id);
        void AddPlayerToGame(string id, Team team);
        void RemoveTeamFromGame(string id, string teamId);
        void endGame(string name, Game game);
    }
}

