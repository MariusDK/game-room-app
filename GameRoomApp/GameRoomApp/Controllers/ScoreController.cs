using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameRoomApp.Controllers
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
        [HttpGet("{id}", Name = "Get")]
        public Score GetScore(string id)
        {
            return _scoreRepository.GetScoreById(id);
        }
        [HttpPut("id/{id}")]
        public string PutScore(string id, [FromBody] Score score)
        {

            var result = string.Empty;
            var existentScore = _scoreRepository.GetScoreById(id);

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
        [HttpDelete("id/{id}")]
        public string DeleteScore(string id)
        {
            var result = string.Empty;
            var existentScore = _scoreRepository.GetScoreById(id);

            if (existentScore != null)
            {

                _scoreRepository.RemoveScore(id);
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
