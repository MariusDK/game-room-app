using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.PlayerRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            this._playerRepository = playerRepository;
        }
        [HttpGet]
        public IEnumerable<Player> GetAllPlayers()
        {
            return _playerRepository.GetAllPlayers();
        }
        [HttpGet("id/{id}", Name = "GetPlayerById")]
        public Player GetPlayerById(string id)
        {
            ObjectId Id = new ObjectId(id);
            return _playerRepository.GetPlayerById(Id);
        }
        [HttpGet("name/{name}", Name = "GetPlayerByName")]
        public Player GetPlayerByName(String name)
        {
            var player = default(Player);
            if (!string.IsNullOrEmpty(name))
            {
                return _playerRepository.GetPlayerByName(name);
            }
            return player;
        }
        [HttpPost]
        public string Post([FromBody]Player player)
        {
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerByName(player.Name);
            if (existentPlayer == null)
            {
                _playerRepository.InsertPlayer(player);
                result = "Insert Working!";
            }
            else
            {
                result = $"Player {player.Name} exists!";
            }
            return result;
        }
        [HttpPut("id/{id}")]
        public string PutById(string id, [FromBody] Player player)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerById(Id);
            if (existentPlayer != null)
            {
                player.Id = id;
                _playerRepository.UpdatePlayer(player);
                result = "Update Working!";
            }
           else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
        [HttpPut("name/{name}")]
        public string PutByName(string name, [FromBody] Player player)
        {
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerByName(name);

            if (existentPlayer != null)
            {
                _playerRepository.UpdatePlayerByName(name,player);
                result = "Update Working!";
            }
            else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
        [HttpDelete("id/{id}")]
        public string DeleteById(string id)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerById(Id);

            if (existentPlayer != null)
            {
                _playerRepository.RemovePlayer(Id);
                result = "Delete Working!";
            }
            else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
        [HttpDelete("name/{name}")]
        public string DeleteByName(string name)
        {
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerByName(name);

            if (existentPlayer != null)
            {
                _playerRepository.RemovePlayerByName(name);
                result = "Delete Working!";
            }
            else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
    }
}
