﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.DartsCricketRepository;
using GameRoomApp.providers.DartsX01Repository;
using GameRoomApp.providers.GameRepository;
using GameRoomApp.providers.PlayerRepository;
using GameRoomApp.providers.ScoreRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IScoreRepository _scoreRepository;
        private readonly IPlayerRepository _playerRepository;
        public GameController(IScoreRepository scoreRepository, IGameRepository gameRepository,
            IPlayerRepository playerRepository)
        {
            this._gameRepository = gameRepository;
            this._scoreRepository = scoreRepository;
            this._playerRepository = playerRepository;
        }
        [HttpGet]
        public IEnumerable<Game> GetAllGames()
        {
            return _gameRepository.GetAllGames();
        }
        [HttpGet("history")]
        public IEnumerable<Game> GetGameHistory()
        {
            return _gameRepository.GetGameHistory();
        }
        [HttpGet("leaderboard/{id}",Name = "Leaderboard")]
        public IEnumerable<Score> GetGameLeaderboard(string id)
        {
            ObjectId objectId = new ObjectId(id);
            var existentGame = _gameRepository.GetGameById(objectId);
            if (existentGame != null)
            {
                return _scoreRepository.LeaderboardForGame(existentGame);
            }
            return null;
        }       
        [HttpGet]
        [ActionName(nameof(GetGamesByType))]
        [ExactQueryParam("type")]
        public IEnumerable<Game> GetGamesByType(string type)
        {
            return _gameRepository.GetGamesByType(type);
        }
        [HttpGet]
        [ActionName(nameof(GetGamesByPlayer))]
        [ExactQueryParam("playerId")]
        public IEnumerable<Game> GetGamesByPlayer(string playerId)
        {
            ObjectId idObject = new ObjectId(playerId);
            var player = _playerRepository.GetPlayerById(idObject);
            if (player != null)
            {
                return _gameRepository.GetGamesByPlayer(player);
            }
            return null;
        }
        [HttpGet]
        [ActionName(nameof(GetGameById))]
        [ExactQueryParam("gameId")]
        public Game GetGameById(string gameId)
        {
            ObjectId idObject = new ObjectId(gameId);
            return _gameRepository.GetGameById(idObject);
        }
        [HttpGet]
        [ActionName(nameof(GetGameByName))]
        [ExactQueryParam("gameName")]
        public Game GetGameByName(string gameName)
        {
            return _gameRepository.GetGameByName(gameName);
        }
        [HttpPost]
        public string Post([FromBody] Game game)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(game.Name);
            if (existentGame == null)
            {
                _gameRepository.InsertGame(game);
                _scoreRepository.InsertScoreForAllPlayer(game);
                result = "Insert Working!";
            }
            else
            {
                result = $"Game {game.Name} exists!";
            }
            return result;
        }

        // PUT: api/Game/5
        [HttpPut]
        [ExactQueryParam("gameId")]
        public string PutById(string gameId, [FromBody] Game game)
        {
            ObjectId idObject = new ObjectId(gameId);
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameById(idObject);
            game.Id = gameId;
            if (existentGame != null)
            {
                _gameRepository.UpdateGameById(game);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("gameName")]
        public string PutByName(string gameName, [FromBody] Game game)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(gameName);
            if (existentGame != null)
            {
                _gameRepository.UpdateGameByName(gameName,game);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("playerId")]
        public string PutPlayer(string playerId, [FromBody] Team team)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(playerId);
            var existentGame = _gameRepository.GetGameById(idObject);

            if (existentGame != null)
            {
                _gameRepository.AddPlayerToGame(playerId, team);
                var game = _gameRepository.GetGameById(idObject);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("gameId")]
        public string PutEliminationPlayer(string gameId, [FromBody] string idTeam)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(gameId);
            var existentGame = _gameRepository.GetGameById(idObject);

            if (existentGame != null)
            {
                _gameRepository.RemoveTeamFromGame(id, idTeam);
                Game game = _gameRepository.GetGameById(idObject);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("gameId")]
        public string DeleteById(string gameId)
        {
            var result = string.Empty;
            ObjectId idObject = new ObjectId(gameId);
            var existentGame = _gameRepository.GetGameById(idObject);

            if (existentGame != null)
            {
                _gameRepository.RemoveGameById(idObject);
                _scoreRepository.RemoveScoreByGame(existentGame);
                result = "Delete Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpDelete("name/{name}")]
        [ExactQueryParam("gameName")]
        public string DeleteByName(string gameName)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(gameName);

            if (existentGame != null)
            {
                _gameRepository.RemoveGameByName(gameName);
                _scoreRepository.RemoveScoreByGame(existentGame);
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
