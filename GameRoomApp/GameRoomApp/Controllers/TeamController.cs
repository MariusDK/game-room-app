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

namespace GameRoomApp.Controllers
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
        // GET: api/Team
        [HttpGet]
        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepository.GetAllTeams();
        }

        // GET: api/Team/5
        [HttpGet("name/{name}", Name = "GetByName")]
        public Team GetTeamByName(string name)
        {
            var team = default(Team);
            if (!string.IsNullOrEmpty(name))
            {
                return _teamRepository.GetTeamByName(name);
            }
            return team;
        }
        [HttpGet("id/{id}", Name = "GetById")]
        public Team GetTeamById(string id)
        {
            
            var team = default(Team);
            if (!string.IsNullOrEmpty(id))
            {
                ObjectId Id = new ObjectId(id);
                return _teamRepository.GetTeam(Id);
            }
            return team;
        }
        [HttpGet("players/{id}",Name = "GetPlayers")]
        public IEnumerable<Player> GetPlayers(string id)
        {

            var team = default(Team);
            if (!string.IsNullOrEmpty(id))
            {
                ObjectId Id = new ObjectId(id);
                team = _teamRepository.GetTeam(Id);
                return team.Players;
            }
            return null;
        }
        // POST: api/Team
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
       
        // PUT: api/Team/5
        [HttpPut("name/{name}")]
        public string PutByName(string name, [FromBody] Team team)
        {
            var result = string.Empty;
            var existentTeam = _teamRepository.GetTeamByName(name);

            if (existentTeam != null)
            {

                _teamRepository.UpdateTeamByName(name,team);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpPut("id/{id}")]
        public string PutById(string id, [FromBody] Team team)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentTeam = _teamRepository.GetTeam(Id);
            
            if (existentTeam != null)
            {
                team.Id = Id;
                _teamRepository.UpdateTeam(team);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpPut("addPlayer/{id}")]
        public string PutPlayer(string id, [FromBody] Player player)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentTeam = _teamRepository.GetTeam(Id);

            if (existentTeam != null)
            {
                _teamRepository.AddPlayerToTeam(Id, player);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpPut("removePlayer/{id}")]
        public string PutDeletePlayer(string id, [FromBody] string idPlayer)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentTeam = _teamRepository.GetTeam(Id);

            if (existentTeam != null)
            {
                _teamRepository.RemovePlayerFromTeam(Id, idPlayer);
                result = "Update Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("name/{name}")]
        public string DeleteByName(string name)
        {
            var result = string.Empty;
            var existentTeam = _teamRepository.GetTeamByName(name);

            if (existentTeam != null)
            {
                _teamRepository.RemoveTeamByName(name);
                result = "Delete Working!";
            }
            else
            {
                result = $"Team don't exists!";
            }
            return result;
        }
        [HttpDelete("id/{id}")]
        public string DeleteById(string id)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentPlayer = _teamRepository.GetTeam(Id);

            if (existentPlayer != null)
            {
                _teamRepository.RemoveTeam(Id);
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
