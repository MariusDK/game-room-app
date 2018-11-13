using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.TeamRepository
{
    public class TeamProvider : ITeamRepository
    {
        private readonly ITeamContext _teamContext;

        public TeamProvider(ITeamContext teamContext)
        {
            _teamContext = teamContext;
        }
        public void InsertTeam(Team a)
        {
            _teamContext.Team.InsertOne(a);
        }
        public Team GetTeam(ObjectId Id)
        {
           
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Eq("Id",Id);
            var cursor = _teamContext.Team.Find(idFilter);
            Team team = cursor.FirstOrDefault();
            return team;
        }
        public IEnumerable<Team> GetAllTeams()
        {
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Empty;
            var cursor = _teamContext.Team.Find(idFilter);
            List<Team> teams = cursor.ToList();
            return teams;
        }
        public void UpdateTeam(Team team)
        {
            FilterDefinitionBuilder<Team> Fbuilder = Builders<Team>.Filter;
            var UBuilder = Builders<Team>.Update;
            var idFilter = Fbuilder.Eq("Id", team.Id);
            var updateDefinition = UBuilder.Set("Name", team.Name).Set("Players", team.Players).Set("Game",team.Game);
            var cursor = _teamContext.Team.UpdateOne(idFilter,updateDefinition);
        }
        public void RemoveTeam(ObjectId Id)
        {
            
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Eq("Id",Id);
            _teamContext.Team.DeleteOne(idFilter);
        }
        public void AddPlayerToTeam(ObjectId id,Player player)
        {
            Team team = GetTeam(id);
            List <Player> players= team.Players;
            players.Add(player);
            team.Players = players;
            UpdateTeam(team);
        }

    }
}
