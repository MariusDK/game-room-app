using GameRoomApp.classes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.GameRepository
{
    public interface IGameRepository
    {
        void InsertGame(Game game);
        Game GetSpecificGame(ObjectId objectId);
        IEnumerable<Game> GetAllGames();
        void UpdateGame(Game game);
        void RemoveGame(ObjectId Id);
        IEnumerable<Game> GetGamesByPlayer(ObjectId Id);
        IEnumerable<Game> GetGameHistory();
        IEnumerable<Game> GetGameWithType(Type type);
    }
}
}
