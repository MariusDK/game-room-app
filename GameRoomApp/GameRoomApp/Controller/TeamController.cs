using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
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
        public TeamController(ITeamRepository teamRepository)
        {
            this._teamRepository = teamRepository;
        }
        [HttpGet]
        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepository.GetAllTeams();
        }
        [HttpGet]
        [ActionName(nameof(GetTeamByName))]
        [ExactQueryParam("playerName")]
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
        public string Post([FromBody] Team team)
        {
            var result = string.Empty;
            var existentTeam = _teamRepository.GetTeamByName(team.Name);
            if (existentTeam == null)
            {
                _teamRepository.InsertTeam(team);
                result = "Insert Working!";
            }
            else
            {
                result = $"Team {team.Name} exists!";
            }
            return result;
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
                team.Id = idObject;
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
