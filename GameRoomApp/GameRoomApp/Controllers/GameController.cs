using System;
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

namespace GameRoomApp.Controllers
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
        [HttpGet("history/")]
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
        [HttpGet("type/{type}", Name = "GetGameByType")]
        public IEnumerable<Game> GetGamesByType(string type)
        {
            return _gameRepository.GetGamesByType(type);
        }
        [HttpGet("idPlayer/{id}", Name = "GetGameByPlayer")]
        public IEnumerable<Game> GetGamesByPlayer(string id)
        {
            ObjectId Id = new ObjectId(id);
            var player = _playerRepository.GetPlayerById(Id);
            if (player != null)
            {
                return _gameRepository.GetGamesByPlayer(player);
            }
            return null;
        }
        [HttpGet("id/{id}", Name = "GetGameById")]
        public Game GetGameById(string id)
        {
            ObjectId Id = new ObjectId(id);
            return _gameRepository.GetGameById(Id);
        }
        [HttpGet("name/{name}", Name = "GetGameByName")]
        public Game GetGameByName(string name)
        {
            return _gameRepository.GetGameByName(name);
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
        [HttpPut("id/{id}")]
        public string PutById(string id, [FromBody] Game game)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameById(Id);
            game.Id = id;
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
        [HttpPut("name/{name}")]
        public string PutByName(string name, [FromBody] Game game)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(name);
            if (existentGame != null)
            {
                _gameRepository.UpdateGameByName(name,game);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpPut("addPlayer/{id}")]
        public string PutPlayer(string id, [FromBody] Player player)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentGame = _gameRepository.GetGameById(Id);

            if (existentGame != null)
            {
                _gameRepository.AddPlayerToGame(id, player);
                var game = _gameRepository.GetGameById(Id);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpPut("removePlayer/{id}")]
        public string PutEliminationPlayer(string id, [FromBody] string idPlayer)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentGame = _gameRepository.GetGameById(Id);

            if (existentGame != null)
            {
                _gameRepository.RemovePlayerFromGame(id, idPlayer);
                Game game = _gameRepository.GetGameById(Id);
                _scoreRepository.UpdateScoreByGame(game,existentGame);
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpDelete("id/{id}")]
        public string DeleteById(string id)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(id);
            var existentGame = _gameRepository.GetGameById(Id);

            if (existentGame != null)
            {
                _gameRepository.RemoveGameById(Id);
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
        public string DeleteByName(string name)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(name);

            if (existentGame != null)
            {
                _gameRepository.RemoveGameByName(name);
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
