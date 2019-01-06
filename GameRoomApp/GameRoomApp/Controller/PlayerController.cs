﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
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
        [HttpGet]
        [ActionName(nameof(GetPlayerById))]
        [ExactQueryParam("playerId")]
        public Player GetPlayerById(string playerId)
        {
            ObjectId idObject = new ObjectId(playerId);
            return _playerRepository.GetPlayerById(idObject);
        }
        [HttpGet]
        [ActionName(nameof(GetPlayerByName))]
        [ExactQueryParam("playerName")]
        public Player GetPlayerByName(String playerName)
        {
            var player = default(Player);
            if (!string.IsNullOrEmpty(playerName))
            {
                return _playerRepository.GetPlayerByName(playerName);
            }
            return player;
        }
        [HttpGet]
        [ActionName(nameof(GetPlayerByUsername))]
        [ExactQueryParam("username")]
        public Player GetPlayerByUsername(String username)
        {
            var player = default(Player);
            if (!string.IsNullOrEmpty(username))
            {
                return _playerRepository.GetPlayerByUsername(username);
            }
            return player;
        }
        [HttpPost]
        public string Post([FromBody]Player player)
        {
            var result = string.Empty;
            string password = player.Password;
            string passwordHash = _playerRepository.GetHashString(password);
            string username = player.Username;
            player.Password = passwordHash;
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
        [HttpPost("login")]
        public Player Login([FromBody]Player player)
        {
            var result = string.Empty;
            string password = player.Password;
            string passwordHash = _playerRepository.GetHashString(password);
            string username = player.Username;
            var existentPlayer = _playerRepository.login(username,passwordHash);
            return existentPlayer;
        }
        [HttpPut]
        [ExactQueryParam("playerId")]
        public string PutById(string playerId, [FromBody] Player player)
        {
            string password = player.Password;
            string passwordHash = _playerRepository.GetHashString(password);
            player.Password = passwordHash;
            ObjectId idObject = new ObjectId(playerId);
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerById(idObject);
            if (existentPlayer != null)
            {
                player.Id = playerId;
                _playerRepository.UpdatePlayer(player);
                result = "Update Working!";
            }
           else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("playerName")]
        public string PutByName(string playerName, [FromBody] Player player)
        {
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerByName(playerName);

            if (existentPlayer != null)
            {
                _playerRepository.UpdatePlayerByName(playerName,player);
                result = "Update Working!";
            }
            else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("playerId")]
        public string DeleteById(string playerId)
        {
            ObjectId idObject = new ObjectId(playerId);
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerById(idObject);

            if (existentPlayer != null)
            {
                _playerRepository.RemovePlayer(idObject);
                result = "Delete Working!";
            }
            else
            {
                result = $"Player don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("playerName")]
        public string DeleteByName(string playerName)
        {
            var result = string.Empty;
            var existentPlayer = _playerRepository.GetPlayerByName(playerName);

            if (existentPlayer != null)
            {
                _playerRepository.RemovePlayerByName(playerName);
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
