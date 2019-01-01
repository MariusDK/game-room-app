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
using GameRoomApp.providers.TeamRepository;
using GameRoomApp.Util;
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
        private readonly ITeamRepository _teamRepository;
        private PredictionGame _predicitonGame;
        public GameController(IScoreRepository scoreRepository, IGameRepository gameRepository,
            IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            this._gameRepository = gameRepository;
            this._scoreRepository = scoreRepository;
            this._playerRepository = playerRepository;
            this._teamRepository = teamRepository;
            _predicitonGame = new PredictionGame();
  
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
        [HttpGet]
        [ActionName(nameof(GetGameLeaderboard))]
        [ExactQueryParam("gameIdl")]
        public IEnumerable<Score> GetGameLeaderboard(string gameIdl)
        {
            ObjectId objectId = new ObjectId(gameIdl);
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
        [ActionName(nameof(GetGamesUnfinishByPlayer))]
        [ExactQueryParam("uplayerId")]
        public IEnumerable<Game> GetGamesUnfinishByPlayer(string uplayerId)
        {
            ObjectId idObject = new ObjectId(uplayerId);
            var player = _playerRepository.GetPlayerById(idObject);
            List<Game> games = new List<Game>();
            List<Game> gamesUnfinish = new List<Game>();
            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                foreach (Team team in teams)
                {
                    foreach (Game game in _gameRepository.GetGamesByTeam(team))
                    {
                        games.Add(game);
                    }
                }
                foreach (Game g in games)
                {
                    if (g.EndOn == null)
                    {
                        gamesUnfinish.Add(g);
                    }
                }
            }
            return gamesUnfinish;
        }
        [HttpGet]
        [ActionName(nameof(GetGamesFinishByPlayer))]
        [ExactQueryParam("fplayerId")]
        public IEnumerable<Game> GetGamesFinishByPlayer(string fplayerId)
        {
            ObjectId idObject = new ObjectId(fplayerId);
            List<Game> games = new List<Game>();
            List<Game> gamesFinish = new List<Game>();
            var player = _playerRepository.GetPlayerById(idObject);
            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                foreach (Team team in teams)
                {
                    foreach (Game game in _gameRepository.GetGamesByTeam(team))
                    {
                        games.Add(game);
                    }
                }
                foreach (Game g in games)
                {
                    if (g.EndOn != null)
                    {
                        gamesFinish.Add(g);
                    }
                }
            }
            return gamesFinish;
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
        [HttpGet]
        [ActionName(nameof(GetPredictionGameAsync))]
        [ExactQueryParam("imgAbout")]
        public async Task<string> GetPredictionGameAsync(string imgAbout)
        {
            string img = _gameRepository.GetImage(imgAbout);
            string response = await _predicitonGame.MakePredictionRequest(img);
            string result = _predicitonGame.ParseJsonToGetPrediction(response);
            return result;
        }

        [HttpPost]
        public string Post([FromBody] Game game)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(game.Name);
            if (existentGame == null)
            {
                _gameRepository.InsertGame(game);
                _scoreRepository.InsertScoreForAllPlayers(game);
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
                result = "Update Working!";
            }
            else
            {
                result = $"Game don't exists!";
            }
            return result;
        }
        [HttpPut]
        [ExactQueryParam("name")]
        public string finishGame(string name, [FromBody] Game game)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(name);
            game.EndOn = DateTime.Now;
            if (existentGame != null)
            {
                List<Score> scores = _scoreRepository.GetScoresForGame(existentGame);
                foreach (Score score in scores)
                {
                    score.Game = game;
                    _scoreRepository.UpdateScore(score);
                }
                //_scoreRepository.UpdateScoreByGame(game,existentGame);
                _gameRepository.UpdateGameById(game);
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
                _gameRepository.RemoveTeamFromGame(gameId, idTeam);
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
