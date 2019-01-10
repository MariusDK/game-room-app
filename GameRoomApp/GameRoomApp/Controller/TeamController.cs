using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.GameRepository;
using GameRoomApp.providers.TeamRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IGameRepository _gameRepository;
        public TeamController(ITeamRepository teamRepository, IGameRepository gameRepository)
        {
            this._teamRepository = teamRepository;
            this._gameRepository = gameRepository;
        }
        [HttpGet]
        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepository.GetAllTeams();
        }
        [HttpGet]
        [ActionName(nameof(GetTeamByName))]
        [ExactQueryParam("idPlayer")]
        public IEnumerable<Team> GetAllTeamsOfUser(string idPlayer)
        {
            Player player = new Player();
            player.Id = idPlayer;
            return _teamRepository.GetTeamByPlayer(player);
        }
        [HttpGet]
        [ActionName(nameof(GetTeamByName))]
        [ExactQueryParam("teamName")]
        public Team GetTeamByName(string teamName)
        {
            var team = default(Team);
            if (!string.IsNullOrEmpty(teamName))
            {
                return _teamRepository.GetTeamByName(teamName);
            }
            return team;
        }
        [HttpGet]
        [ActionName(nameof(GetTeamById))]
        [ExactQueryParam("teamId")]
        public Team GetTeamById(string teamId)
        {
            
            var team = default(Team);
            if (!string.IsNullOrEmpty(teamId))
            {
                ObjectId idObject = new ObjectId(teamId);
                return _teamRepository.GetTeam(idObject);
            }
            return team;
        }
        [HttpGet]
        [ActionName(nameof(GetPlayers))]
        [ExactQueryParam("teamId")]
        public IEnumerable<Player> GetPlayers(string teamId)
        {

            var team = default(Team);
            if (!string.IsNullOrEmpty(teamId))
            {
                ObjectId idObject = new ObjectId(teamId);
                team = _teamRepository.GetTeam(idObject);
                return team.Players;
            }
            return null;
        }
        [HttpPost]
        public Team Post([FromBody] Team team)
        {
            if (team.Players.Count == 0)
            {
                return null;
            }
            if (team.Name != "") {
                Team existentTeam = _teamRepository.GetTeamByName(team.Name);
                if (existentTeam == null)
                {
                    Team teamResult = _teamRepository.InsertTeam(team);
                    return teamResult;
                }
                return null;
            }
            else
            {
                Team teamResult = _teamRepository.InsertTeam(team);
                return teamResult;
            }
            
        }
        [HttpPut]
        [ExactQueryParam("teamName")]
        public string PutByName(string teamName, [FromBody] Team team)
        {
            var result = string.Empty;
            var existentTeam = _teamRepository.GetTeamByName(teamName);

            if (existentTeam != null)
            {

                _teamRepository.UpdateTeamByName(teamName,team);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("teamId")]
        public string PutById(string teamId, [FromBody] Team team)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(teamId);
            var existentTeam = _teamRepository.GetTeam(idObject);
            
            if (existentTeam != null)
            {
                team.Id =teamId;
                _teamRepository.UpdateTeam(team);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("teamId")]
        public string PutPlayer(string teamId, [FromBody] Player player)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(teamId);
            var existentTeam = _teamRepository.GetTeam(idObject);

            if (existentTeam != null)
            {
                _teamRepository.AddPlayerToTeam(idObject, player);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("teamId")]
        public string PutDeletePlayer(string teamId, [FromBody] string idPlayer)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(teamId);
            var existentTeam = _teamRepository.GetTeam(idObject);

            if (existentTeam != null)
            {
                _teamRepository.RemovePlayerFromTeam(idObject, idPlayer);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("teamName")]
        public string DeleteByName(string teamName)
        {
            var result = string.Empty;
            var existentTeam = _teamRepository.GetTeamByName(teamName);

            if (existentTeam != null)
            {
                _teamRepository.RemoveTeamByName(teamName);
                result = "Delete Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("teamId")]
        public string DeleteById(string teamId)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(teamId);
            var existentTeam = _teamRepository.GetTeam(idObject);

            if (existentTeam != null)
            {
                _teamRepository.RemoveTeam(idObject);
                _gameRepository.RemoveGamesByTeam(teamId);
                result = "Delete Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
    }
}
