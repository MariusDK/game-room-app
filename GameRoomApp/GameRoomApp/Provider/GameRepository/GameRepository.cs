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
            FilterDefinitionBuilder<Game> gFilter = Builders<Game>.Filter;
            var uBuilder = Builders<Game>.Update;
            var idFilter = gFilter.Eq("Id", game.Id);
            var updateDefinition = uBuilder.Set("Name",game.Name).Set("Type",game.Type).Set("Teams",game.Teams).
                Set("StartOn",game.StartOn).Set("EndOn",game.EndOn).
                Set("EmbarrassingMoments",game.EmbarrassingMoments).Set("VictoryMoments",game.VictoryMoments);
            var cursor = _gameContext.Games.UpdateOne(idFilter,updateDefinition);
        }
        public void UpdateGameByName(string name,Game game)
        {
            FilterDefinitionBuilder<Game> gFilter = Builders<Game>.Filter;
            var uBuilder = Builders<Game>.Update;
            var nameFilter = gFilter.Eq("Name", name);
            var updateDefinition = uBuilder.Set("Name", game.Name).Set("Type", game.Type).Set("Teams", game.Teams).
                Set("StartOn", game.StartOn).Set("EndOn", game.EndOn).
                Set("EmbarrassingMoments", game.EmbarrassingMoments).Set("VictoryMoments", game.VictoryMoments);
            var cursor = _gameContext.Games.UpdateOne(nameFilter, updateDefinition);
        }
        public void EndGame(string name, Game game)
        {
            DateTime dateTime = DateTime.Today;
            FilterDefinitionBuilder<Game> gFilter = Builders<Game>.Filter;
            var uBuilder = Builders<Game>.Update;
            var nameFilter = gFilter.Eq("Name", name);
            var updateDefinition = uBuilder.Set("Name", game.Name).Set("Type", game.Type).Set("Teams", game.Teams).
                Set("StartOn", game.StartOn).Set("EndOn", dateTime).
                Set("EmbarrassingMoments", game.EmbarrassingMoments).Set("VictoryMoments", game.VictoryMoments);
            var cursor = _gameContext.Games.UpdateOne(nameFilter, updateDefinition);
        }
        public void RemoveGameById(ObjectId id)
        {
            var builder = Builders<Game>.Filter;
            var idFilter = builder.Eq("Id",id);
            _gameContext.Games.DeleteOne(idFilter);
        }
        public void RemoveGameByName(string name)
        {
            var builder = Builders<Game>.Filter;
            var nameFilter = builder.Eq("Name", name);
            _gameContext.Games.DeleteOne(nameFilter);
        }
        public List<Game> GetGamesByTeam(Team team)
        {
            var builder = Builders<Game>.Filter;
            var teamFilter = builder.Eq("Teams", team);
            var cursor = _gameContext.Games.Find(teamFilter);
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
        public void AddPlayerToGame(string id, Team team)
        {
            ObjectId objectId = new ObjectId(id);
            Game game = GetGameById(objectId);
            List<Team> teams = game.Teams;
            teams.Add(team);
            game.Teams = teams;
            UpdateGameById(game);
        }
        public void RemoveTeamFromGame(string id, string teamId)
        {
            ObjectId objectId = new ObjectId(id);
            Game game = GetGameById(objectId);
            List<Team> teams = game.Teams;
            foreach (Team team in teams)
            {
                if (team.Id.Equals(teamId))
                {
                    teams.Remove(team);
                    break;
                }
            }
            game.Teams = teams;
            UpdateGameById(game);
        }
        public string GetImage(string imgData)
        {
            string[] imgDataSplit = imgData.Split(",");
            string imgPosition = imgDataSplit[0];
            string listType = imgDataSplit[1];
            string gameName = imgDataSplit[2];
            List<string> listMoments;
            var builder = Builders<Game>.Filter;
            var nameFilter = builder.Eq("Name", gameName);
            var cursor = _gameContext.Games.Find(nameFilter);
            Game game = cursor.FirstOrDefault();
            if (listType.Equals("vicMoments"))
            {
                listMoments = game.VictoryMoments;
            }
            else {
                listMoments = game.EmbarrassingMoments;
            }
            int position;
            int.TryParse(imgPosition,out position);
            for (int i = 0; i < listMoments.Capacity; i++)
            {
                if (position == i)
                {
                    return listMoments[i];
                }
            }
            return null;
        }
    }
    
}
