﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
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
        private readonly IDartsX01Repository _dartsX01Repository;
        private readonly IDartsCricketRepository _dartsCricketRepository;
        private PredictionGame _predicitonGame;
        public GameController(IScoreRepository scoreRepository, IGameRepository gameRepository,
            IPlayerRepository playerRepository, ITeamRepository teamRepository, IDartsX01Repository dartsX01Repository, IDartsCricketRepository dartsCricketRepository)
        {
            this._gameRepository = gameRepository;
            this._scoreRepository = scoreRepository;
            this._playerRepository = playerRepository;
            this._teamRepository = teamRepository;
            this._dartsX01Repository = dartsX01Repository;
            this._dartsCricketRepository = dartsCricketRepository;
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
        [ActionName(nameof(GetGameLeaderboard))]
        [ExactQueryParam("page")]
        public IEnumerable<Score> GetGame(string gameIdl)
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
        [ActionName(nameof(GetGamesUnfinishByPlayerAndType))]
        [ExactQueryParam("page", "type", "userIdu")]
        public IEnumerable<Game> GetGamesUnfinishByPlayerAndType(int page, string type, string userIdu)
        {
            ObjectId idObject = new ObjectId(userIdu);
            var player = _playerRepository.GetPlayerById(idObject);
            List<Game> unfinishGames = new List<Game>();

            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                unfinishGames = (List<Game>)_gameRepository.PaginationUnfinishGamesByType(page,type,4,teams);
            }
            return unfinishGames;
        }
        [HttpGet]
        [ActionName(nameof(GetGamesFinishByPlayerAndType))]
        [ExactQueryParam("page", "type", "userIdf")]
        public IEnumerable<Game> GetGamesFinishByPlayerAndType(int page, string type, string userIdf)
        {
            ObjectId idObject = new ObjectId(userIdf);
            var player = _playerRepository.GetPlayerById(idObject);
            List<Game> finishGames = new List<Game>();

            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                finishGames = (List<Game>)_gameRepository.PaginationFinishGamesByType(page, type, 4, teams);
            }
            return finishGames;
        }
        [HttpGet]
        [ActionName(nameof(GetGamesUnfinishByPlayer))]
        [ExactQueryParam("page","userIdu")]
        public IEnumerable<Game> GetGamesUnfinishByPlayer(int page, string userIdu)
        {
            ObjectId idObject = new ObjectId(userIdu);
            var player = _playerRepository.GetPlayerById(idObject);
            List<Game> unfinishGames = new List<Game>();

            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                List<Game> games = (List<Game>)_gameRepository.GetUnfinishGamesOfUser(teams);
                unfinishGames = (List<Game>)_gameRepository.PaginationUnfinishGamesOfUser(page,4,games);
            }
            return unfinishGames;
        }
        [HttpGet]
        [ActionName(nameof(GetGamesFinishByPlayer))]
        [ExactQueryParam("page", "userIdf")]
        public IEnumerable<Game> GetGamesFinishByPlayer(int page, string userIdf)
        {
            ObjectId idObject = new ObjectId(userIdf);
            List<Game> finishGames = new List<Game>();
            var player = _playerRepository.GetPlayerById(idObject);
            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                List<Game> games = (List<Game>)_gameRepository.GetFinishGamesOfUser(teams);
                finishGames = (List<Game>)_gameRepository.PaginationFinishGamesOfUser(page,4,games);
            }
            return finishGames;
        }
        [HttpGet]
        [ActionName(nameof(GetOrderGamesUnfinishByPlayer))]
        [ExactQueryParam("page", "userIduo")]
        public IEnumerable<Game> GetOrderGamesUnfinishByPlayer(int page, string userIduo)
        {
            ObjectId idObject = new ObjectId(userIduo);
            var player = _playerRepository.GetPlayerById(idObject);
            List<Game> unfinishGames = new List<Game>();

            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                List<Game> games = (List<Game>)_gameRepository.GetOrderedUnfinishGamesOfUser(teams);
                unfinishGames = (List<Game>)_gameRepository.PaginationUnfinishGamesOfUser(page, 4, games);
            }
            return unfinishGames;
        }
        [HttpGet]
        [ActionName(nameof(GetOrderGamesFinishByPlayer))]
        [ExactQueryParam("page", "userIdfo")]
        public IEnumerable<Game> GetOrderGamesFinishByPlayer(int page, string userIdfo)
        {
            ObjectId idObject = new ObjectId(userIdfo);
            var player = _playerRepository.GetPlayerById(idObject);
            List<Game> unfinishGames = new List<Game>();

            if (player != null)
            {
                List<Team> teams = _teamRepository.GetTeamByPlayer(player);
                List<Game> games = (List<Game>)_gameRepository.GetOrderedFinishGamesOfUser(teams);
                unfinishGames = (List<Game>)_gameRepository.PaginationUnfinishGamesOfUser(page, 4, games);
            }
            return unfinishGames;
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
        public string FinishGame(string name, [FromBody] Game game)
        {
            var result = string.Empty;
            var existentGame = _gameRepository.GetGameByName(name);
            var equalityIndice = 0;
            game.EndOn = DateTime.Now;
            if (existentGame != null)
            {
                List<Score> scores = _scoreRepository.GetScoresForGame(existentGame);
                Score winnerScore = new Score();
                winnerScore.Value = 0;
                //find who is the winner
                if (game.Type.Equals("Darts/X01"))
                {
                    var leaderboardDartsX01 = _dartsX01Repository.LeaderboardInGameDartsX01(game, scores);
                    var winnerDartsX01 = leaderboardDartsX01.First();
                    foreach (Score score in scores)
                    {
                        if (score.Team.Id == winnerDartsX01.Key.Id)
                        {
                            winnerScore = score;
                            winnerScore.Value = 1;
                        }
                    }
                    _scoreRepository.UpdateScore(winnerScore);
                }
                else
                {
                    foreach (Score score in scores)
                    {
                        score.Game = game;
                        if (winnerScore.Value < score.Value)
                        {
                            winnerScore = score;
                        }

                    }
                    foreach (Score score in scores)
                    {
                        if (winnerScore.Value == score.Value)
                        {
                            equalityIndice++;
                        }
                    }
                }
                if (equalityIndice != scores.Count) {
                    foreach (Score score in scores)
                    {

                        int k = 0;
                        score.Game = game;
                        if (winnerScore.Id == score.Id)
                        {
                            double ChanceOfVictory = winnerScore.ChanceOfVictory + 3;
                            if (ChanceOfVictory >= 100)
                            {
                                ChanceOfVictory = ChanceOfVictory - (ChanceOfVictory - 100);
                            }
                            score.ChanceOfVictory = ChanceOfVictory;
                            k++;
                        }
                        else
                        {
                            if (score.ChanceOfVictory != 0)
                            {
                                score.ChanceOfVictory = score.ChanceOfVictory - 3;
                            }
                        }

                        _scoreRepository.UpdateScore(score);
                    }
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
