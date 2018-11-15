using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.DartsCricketRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DartsCricketController : ControllerBase
    {
        private readonly IDartsCricketRepository _dartsCricketRepository;
        public DartsCricketController(IDartsCricketRepository dartsCricketRepository)
        {
            this._dartsCricketRepository = dartsCricketRepository;
        }
        [HttpGet]
        public IEnumerable<DartsCricket> GetAllDartsCricket()
        {
            return _dartsCricketRepository.GetAllDartsCricket();
        }
        [HttpGet("id/{id}", Name = "GetDartsCricket")]
        public DartsCricket GetDartsCricket(string id)
        {
            ObjectId Id = new ObjectId(id);
            return _dartsCricketRepository.GetDartsCricket(Id);
        }
        [HttpPost]
        public string PostDartsCricket([FromBody] DartsCricket dartsCricket)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(dartsCricket.Id);
            var existentDartsCricket = _dartsCricketRepository.GetDartsCricket(Id);
            if (existentDartsCricket == null)
            {
                _dartsCricketRepository.InsertDartsCricket(dartsCricket);
                result = "Insert Working!";
            }
            else
            {
                result = $"Darts cricket game exists!";
            }
            return result;
        }
        [HttpPut("id/{id}")]
        public string PutDartsCricket(string id, [FromBody] DartsCricket dartsCricket)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentDartsCricket = _dartsCricketRepository.GetDartsCricket(Id);
            dartsCricket.Id = id;

            if (existentDartsCricket != null)
            {
                _dartsCricketRepository.UpdateDartsCricket(dartsCricket);
                result = "Update Working!";
            }
            else
            {
                result = $"Darts Cricket game don't exists!";
            }
            return result;
        }
        [HttpDelete("id/{id}")]
        public string DeleteDartsCricket(string id)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentDartsCricket = _dartsCricketRepository.GetDartsCricket(Id);

            if (existentDartsCricket != null)
            {
                _dartsCricketRepository.RemoveDartsCricket(Id.ToString());
                result = "Delete Working!";
            }
            else
            {
                result = $"Darts cricket game don't exists!";
            }
            return result;
        }
    }
}
