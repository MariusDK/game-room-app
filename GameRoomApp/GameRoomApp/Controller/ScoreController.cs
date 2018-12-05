using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameRoomApp.providers.GameRepository;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly IGameRepository _gameRepository;
        public ScoreController(IScoreRepository scoreRepository,IGameRepository gameRepository)
        {
            this._scoreRepository = scoreRepository;
            this._gameRepository = gameRepository;
        }
        [HttpGet]
        [ActionName("GetScore")]
        [ExactQueryParam("scoreId")]
        public Score GetScore(string scoreId)
        {
            return _scoreRepository.GetScoreById(scoreId);
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
