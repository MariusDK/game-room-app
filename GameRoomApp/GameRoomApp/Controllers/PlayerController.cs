using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.PlayerRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controllers
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
        public IEnumerable<Player> Get()
        {

            Player p = new Player();
            p.Name = "marius";
            p.Password = "ana";
            p.Age = 22;
            p.Username = "boss";
            _playerRepository.InsertPlayer(p);
            return _playerRepository.GetAllPlayers();
        }
        //[HttpGet("{id}", Name = "Get")]
        //public Player Get(ObjectId Id)
        //{
        //    //verificare
        //    return _playerRepository.GetPlayer(Id);
        //}
        [HttpGet("{name}", Name = "Get")]
        public Player Get(String name)
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
        //[HttpPut("{id}")]
        //public void Put(ObjectId id, [FromBody] Player player)
        //{
        //    var result = string.Empty;
        //    var existentPlayer = _playerRepository.GetPlayer(id);

        //    if (existentPlayer != null)
        //    {
        //        player.Id = id;
        //        _playerRepository.UpdatePlayer(player);
        //        result = "Update Working!";
        //    }
        //    else
        //    {
        //        result = $"Player don't exists!";
        //    }
        //}
        [HttpPut("{name}")]
        public string Put(string name, [FromBody] Player player)
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
        //[HttpDelete("id")]
        //public void Delete(ObjectId id)
        //{
        //    var result = string.Empty;
        //    var existentPlayer = _playerRepository.GetPlayer(id);

        //    if (existentPlayer != null)
        //    {
        //        _playerRepository.RemovePlayer(id);
        //        result = "Delete Working!";
        //    }
        //    else
        //    {
        //        result = $"Player don't exists!";
        //    }
        //}
        [HttpDelete("{name}")]
        public string Delete(string name)
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
