﻿using GameRoomApp.DataModel;
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
            var idFilter = builder.Eq("Id", objectId);
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
        public Team GetPlayerTeam(Game game, string playerId)
        {
            foreach (Team team in game.Teams)
            {
                foreach (Player player in team.Players)
                {
                    if (player.Id.Equals(playerId))
                    {
                        return team;
                    }
                }
            }
            return null;

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
            var updateDefinition = uBuilder.Set("Name", game.Name).Set("Type", game.Type).Set("Teams", game.Teams).
                Set("StartOn", game.StartOn).Set("EndOn", game.EndOn).
                Set("EmbarrassingMoments", game.EmbarrassingMoments).Set("VictoryMoments", game.VictoryMoments);
            var cursor = _gameContext.Games.UpdateOne(idFilter, updateDefinition);
        }
        public void UpdateGameByName(string name, Game game)
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
            var idFilter = builder.Eq("Id", id);
            _gameContext.Games.DeleteOne(idFilter);
        }
        public void RemoveGameByName(string name)
        {
            var builder = Builders<Game>.Filter;
            var nameFilter = builder.Eq("Name", name);
            _gameContext.Games.DeleteOne(nameFilter);
        }
        public void RemoveGamesByTeam(string teamId)
        {
            ObjectId objectId = new ObjectId(teamId);
            var builder = Builders<Game>.Filter;
            var nameFilter = builder.Eq("Teams.Id", objectId);
            _gameContext.Games.DeleteMany(nameFilter);

        }
        public List<Game> GetGamesByTeam(Team team)
        {
            var builder = Builders<Game>.Filter;
            var teamFilter = builder.Eq("Teams.Id", team.Id);
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
        public List<Game> GetGamesByType(string type, List<Game> games)
        {
            List<Game> dartsCricketGames = new List<Game>();
            List<Game> dartsX01Games = new List<Game>();
            List<Game> foosballGames = new List<Game>();
            List<Game> otherGames = new List<Game>();
            foreach (Game game in games)
            {
                if (game.Type == "Darts/Cricket")
                {
                    dartsCricketGames.Add(game);
                }
                else if (game.Type == "Darts/X01")
                {
                    dartsX01Games.Add(game);
                }
                else if (game.Type == "Foosball")
                {
                    foosballGames.Add(game);
                }
                else
                {
                    otherGames.Add(game);
                }
            }
            if (type == "Darts/Cricket")
            {
                return dartsCricketGames;
            }
            if (type == "Darts/X01")
            {
                return dartsX01Games;
            }
            if (type == "Foosball")
            {
                return foosballGames;
            }
            return otherGames;          
        }
        public IEnumerable<Game> PaginationUnfinishGamesByType(int pageNumber, string type, int limitOfElementsInPage, List<Team> teams)
        {
            pageNumber++;
            List<Game> unfinishGames = (List<Game>)GetUnfinishGamesOfUser(teams);
            List<Game> games = (List<Game>)GetGamesByType(type,unfinishGames);
            List<Game> paginateGames = new List<Game>();
            int startPosition = limitOfElementsInPage * (pageNumber - 1);
            int finishPosition = (limitOfElementsInPage - 1) + limitOfElementsInPage * (pageNumber - 1);
            for (int i = startPosition; i < finishPosition + 1; i++)
            {
                if (i == games.Count)
                { break; }
                paginateGames.Add(games[i]);
            }
            return paginateGames;
        }
        public IEnumerable<Game> PaginationFinishGamesByType(int pageNumber, string type, int limitOfElementsInPage, List<Team> teams)
        {
            pageNumber++;
            List<Game> unfinishGames = (List<Game>)GetFinishGamesOfUser(teams);
            List<Game> games = (List<Game>)GetGamesByType(type, unfinishGames);
            List<Game> paginateGames = new List<Game>();
            int startPosition = limitOfElementsInPage * (pageNumber - 1);
            int finishPosition = (limitOfElementsInPage - 1) + limitOfElementsInPage * (pageNumber - 1);
            for (int i = startPosition; i < finishPosition + 1; i++)
            {
                if (i == games.Count)
                { break; }
                paginateGames.Add(games[i]);
            }
            return paginateGames;
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
            int.TryParse(imgPosition, out position);
            for (int i = 0; i < listMoments.Capacity; i++)
            {
                if (position == i)
                {
                    return listMoments[i];
                }
            }
            return null;
        }

        public IEnumerable<Game> PaginationUnfinishGamesOfUser(int pageNumber, int limitOfElementsInPage, List<Game> games)
        {
            pageNumber++;

            List<Game> paginateGames = new List<Game>();
            int startPosition = limitOfElementsInPage * (pageNumber - 1);
            int finishPosition = (limitOfElementsInPage - 1) + limitOfElementsInPage * (pageNumber - 1);
            for (int i = startPosition; i < finishPosition + 1; i++)
            {
                if (i == games.Count)
                { break; }
                paginateGames.Add(games[i]);
            }
            return paginateGames;
        }
        public IEnumerable<Game> PaginationFinishGamesOfUser(int pageNumber, int limitOfElementsInPage, List<Game> games)
        {
            pageNumber++;
            List<Game> paginateGames = new List<Game>();
            int startPosition = limitOfElementsInPage * (pageNumber - 1);
            int finishPosition = (limitOfElementsInPage - 1) + limitOfElementsInPage * (pageNumber - 1);
            for (int i = startPosition; i < finishPosition + 1; i++)
            {
                if (i == games.Count)
                { break; }
                paginateGames.Add(games[i]);
            }
            return paginateGames;
        }

        public IEnumerable<Game> GetUnfinishGamesOfUser(List<Team> teams)
        {
            List<Game> teamGames = new List<Game>();
            List<Game> unfinishGames = new List<Game>();
            foreach (Team team in teams)
            {
                foreach (Game game in GetGamesByTeam(team))
                {
                    teamGames.Add(game);
                }
            }
            foreach (Game g in teamGames)
            {
                if (g.EndOn == null)
                {
                    unfinishGames.Add(g);
                }
            }
            return unfinishGames;
        }
        public IEnumerable<Game> GetFinishGamesOfUser(List<Team> teams)
        {
            List<Game> teamGames = new List<Game>();
            List<Game> finishGames = new List<Game>();
            foreach (Team team in teams)
            {
                foreach (Game game in GetGamesByTeam(team))
                {
                    teamGames.Add(game);
                }
            }
            foreach (Game g in teamGames)
            {
                if (g.EndOn != null)
                {
                    finishGames.Add(g);
                }
            }
            return finishGames;
        }

        public void RemoveSelectedGame(Game game)
        {
           ObjectId objectId = new ObjectId(game.Id);
           RemoveGameById(objectId);    
        }
        public IEnumerable<Game> GetOrderedUnfinishGamesOfUser(List<Team> teams)
        {
            List<Game> unfinishGames = (List<Game>)GetUnfinishGamesOfUser(teams);
            unfinishGames.Reverse();
            return unfinishGames;
        }
        public IEnumerable<Game> GetOrderedFinishGamesOfUser(List<Team> teams)
        {
            List<Game> finishGames = (List<Game>)GetFinishGamesOfUser(teams);
            Game temp = null;

            for (int i = 0; i < finishGames.Count; i++)
            {
                for (int j = 0; j < finishGames.Count - 1; j++)
                {
                    if (finishGames[j].EndOn < finishGames[j + 1].EndOn)
                    {
                        temp = finishGames[j + 1];
                        finishGames[j + 1] = finishGames[j];
                        finishGames[j] = temp;
                    }
                }
            }
            return finishGames;
        }
        public List<Game> GetGamesOfPlayer(List<Team> teams)
        {
            List<Game> games = new List<Game>();
            foreach (Team team in teams)
            {
                List<Game> gamesOfTeam =GetGamesByTeam(team);
                foreach (Game game in gamesOfTeam)
                {
                    foreach (Team gameTeam in game.Teams)
                    {
                        if (gameTeam.Id == team.Id)
                        {
                            gameTeam.Players = team.Players;
                            break;
                        }
                    }
                    games.Add(game);
                    UpdateGameById(game);
                }
            }
            return games;
            
        }
    }  
}
