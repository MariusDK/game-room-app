using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.TeamRepository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ITeamContext _teamContext;

        public TeamRepository(ITeamContext teamContext)
        {
            _teamContext = teamContext;
        }
        public Team InsertTeam(Team team)
        {
            _teamContext.Team.InsertOne(team);
            return team;
        }
        public Team GetTeam(ObjectId id)
        {
           
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Eq("Id",id);
            var cursor = _teamContext.Team.Find(idFilter);
            Team team = cursor.FirstOrDefault();
            return team;
        }
        public Team GetTeamByPlayers(Player[] players)
        {
            var builder = Builders<Team>.Filter;
            var playersFilter = builder.Eq("Players", players);
            var cursor = _teamContext.Team.Find(playersFilter);
            Team team = cursor.FirstOrDefault();
            return team;
        }
        public List<Team> GetTeamByPlayer(Player player)
        {
            var builder = Builders<Team>.Filter;
            var playerFilter = builder.Eq("Players", player);
            var cursor = _teamContext.Team.Find(playerFilter);
            List<Team> team = cursor.ToList();
            return team;
        }
        public Team GetTeamByName(string name)
        {
            var builder = Builders<Team>.Filter;
            var nameFilter = builder.Eq("Name", name);
            var cursor = _teamContext.Team.Find(nameFilter);
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
            FilterDefinitionBuilder<Team> fBuilder = Builders<Team>.Filter;
            var uBuilder = Builders<Team>.Update;
            var idFilter = fBuilder.Eq("Id", team.Id);
            var updateDefinition = uBuilder.Set("Name", team.Name).Set("Players", team.Players);
            var cursor = _teamContext.Team.UpdateOne(idFilter,updateDefinition);
        }
        public void UpdateTeamByName(string name,Team team)
        {
            FilterDefinitionBuilder<Team> fBuilder = Builders<Team>.Filter;
            var uBuilder = Builders<Team>.Update;
            var nameFilter = fBuilder.Eq("Name", team.Name);
            var updateDefinition = uBuilder.Set("Name", team.Name).Set("Players", team.Players);
            var cursor = _teamContext.Team.UpdateOne(nameFilter, updateDefinition);
        }
        public void RemoveTeam(ObjectId id)
        {
            
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Eq("Id",id);
            _teamContext.Team.DeleteOne(idFilter);
        }
        public void RemoveTeamByName(string name)
        {

            var builder = Builders<Team>.Filter;
            var nameFilter = builder.Eq("Name", name);
            _teamContext.Team.DeleteOne(nameFilter);
        }
        public void AddPlayerToTeam(ObjectId id,Player player)
        {
            Team team = GetTeam(id);
            List <Player> players= team.Players;
            players.Add(player);
            team.Players = players;
            UpdateTeam(team);
        }
        public void RemovePlayerFromTeam(ObjectId id, string idPlayer)
        {
            Team team = GetTeam(id);
            List<Player> players = team.Players;
            foreach (Player p in players)
            {
                if (p.Id.Equals(idPlayer))
                {
                    players.Remove(p);
                    break;
                }
            }
            team.Players = players;
            UpdateTeam(team);
        }
        public List<Team> UpdatePlayerTeams(List<Team> teams, Player newPlayer)
        {
            foreach (Team team in teams)
            {
                List<Player> players = team.Players;
                foreach (Player player in players)
                {
                    if (player.Id == newPlayer.Id)
                    {
                        player.Name = newPlayer.Name;
                        player.Password = newPlayer.Password;
                        player.Username = newPlayer.Username;
                        break;
                    }
                }
                UpdateTeam(team);
            }
            return teams;
        }
    }
}
