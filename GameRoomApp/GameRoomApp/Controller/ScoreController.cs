using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreRepository _scoreRepository;
        public ScoreController(IScoreRepository scoreRepository)
        {
            this._scoreRepository = scoreRepository;
        }
        [HttpGet]
        [ActionName("GetScore")]
        [ExactQueryParam("scoreId")]
        public Score GetScore(string scoreId)
        {
            return _scoreRepository.GetScoreById(id);
        }
        [HttpPut]
        [ExactQueryParam("scoreId")]
        public string PutScore(string scoreId, [FromBody] Score score)
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
