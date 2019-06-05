using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameRoomApp.providers.GameRepository;
using GameRoomApp.Util.CART;
using MongoDB.Bson;
using GameRoomApp.providers.PlayerRepository;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        public ScoreController(IScoreRepository scoreRepository,IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            this._scoreRepository = scoreRepository;
            this._gameRepository = gameRepository;
            this._playerRepository = playerRepository;
        }
        [HttpGet]
        [ActionName("GetScore")]
        [ExactQueryParam("scoreId")]
        public Score GetScore(string scoreId)
        {
            return _scoreRepository.GetScoreById(scoreId);
        }
        [HttpGet]
        [ActionName("GetPrediction")]
        [ExactQueryParam("gameId","playerId")]
        public string GetPrediction(string gameId,string playerId)
        {
            List<Score> AllScores = _scoreRepository.GetScoresOfPlayer(playerId);
            List<Score> scores = new List<Score>();

            foreach (Score score in AllScores)
            {
                if (!(score.Game.Id.Equals(gameId)))
                {
                    scores.Add(score);
                }
            }
            List<string> features = new List<string>() { "GameType", "Location", "Opponent", "Age", "Referee" };
            ObjectId gameObId = new ObjectId(gameId);
            Game game = _gameRepository.GetGameById(gameObId);
            Team teamOfPlayer = _gameRepository.GetPlayerTeam(game, playerId);
            List<Player> opponents = new List<Player>();
            foreach (Team team in game.Teams)
            {
                if (!(team.Id.Equals(teamOfPlayer.Id)))
                {
                    foreach (Player opponent in team.Players)
                    {
                        opponents.Add(opponent);
                    }
                }
            }
            Player player = _playerRepository.GetPlayerById(new ObjectId(playerId));
            DecisionTree decisionTree = new DecisionTree(scores,features,opponents,player,game,_scoreRepository);
            return decisionTree.prediction;
        }
        [HttpGet]
        [ActionName("GetScoresOfGame")]
        [ExactQueryParam("gameName")]
        public IEnumerable<Score> GetScoresOfGame(string gameName)
        {
           
            Game game = _gameRepository.GetGameByName(gameName);
            return _scoreRepository.GetScoresForGame(game);
       
        }

        [HttpPut]
        [ExactQueryParam("scoreId")]
        public string Put(string scoreId, [FromBody] Score score)
        {

            var result = string.Empty;
            var existentScore = _scoreRepository.GetScoreById(scoreId);

            if (existentScore != null)
            {

                _scoreRepository.UpdateScore(score);
                result = "Update Working!";
            }
            else
            {
                result = $"Score don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("scoreId")]
        public string DeleteScore(string scoreId)
        {
            var result = string.Empty;
            var existentScore = _scoreRepository.GetScoreById(scoreId);

            if (existentScore != null)
            {
                _scoreRepository.RemoveScore(scoreId);
                result = "Delete Working!";
            }
            else
            {
                result = $"Score don't exists!";
            }
            return result;
        }
    }
}
