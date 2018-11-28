using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.DartsCricketRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controller
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
        [HttpGet]
        [ActionName(nameof(GetDartsCricket))]
        [ExactQueryParam("dartsId")]
        public DartsCricket GetDartsCricket(string dartsId)
        {
            ObjectId idObject = new ObjectId(dartsId);
            return _dartsCricketRepository.GetDartsCricket(idObject);
        }
        [HttpPost]
        public string PostDartsCricket([FromBody] DartsCricket dartsCricket)
        {
            var result = string.Empty;
            ObjectId id = new ObjectId(dartsCricket.Id);
            var existentDartsCricket = _dartsCricketRepository.GetDartsCricket(id);
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
        [HttpPut]
        [ExactQueryParam("dartsId")]
        public string PutDartsCricket(string dartsId, [FromBody] DartsCricket dartsCricket)
        {
            ObjectId idObject = new ObjectId(dartsId);
            var result = string.Empty;
            var existentDartsCricket = _dartsCricketRepository.GetDartsCricket(idObject);
            dartsCricket.Id = dartsId;

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
        [HttpDelete]
        [ExactQueryParam("dartsId")]
        public string DeleteDartsCricket(string dartsId)
        {
            ObjectId idObject = new ObjectId(dartsId);
            var result = string.Empty;
            var existentDartsCricket = _dartsCricketRepository.GetDartsCricket(idObject);

            if (existentDartsCricket != null)
            {
                _dartsCricketRepository.RemoveDartsCricket(idObject.ToString());
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
