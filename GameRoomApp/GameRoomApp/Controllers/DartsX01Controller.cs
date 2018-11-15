using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.DartsX01Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DartsX01Controller : ControllerBase
    {
        private readonly IDartsX01Repository _dartsX01Repository;
        public DartsX01Controller(IDartsX01Repository dartsX01Repository)
        {
            this._dartsX01Repository = dartsX01Repository;
        }
        [HttpGet]
        public IEnumerable<DartsX01> GetAllDartsX01()
        {
            return _dartsX01Repository.GetAllDartsX01();
        }
        [HttpGet("id/{id}", Name = "GetDartsX01")]
        public DartsX01 GetDartsX01(string id)
        {
            ObjectId Id = new ObjectId(id);
            return _dartsX01Repository.GetDartsX01(Id);
        }
        [HttpPost]
        public string PostDartsX01([FromBody] DartsX01 dartsX01)
        {
            var result = string.Empty;
            ObjectId Id = new ObjectId(dartsX01.Id);
            var existentDartsX01 = _dartsX01Repository.GetDartsX01(Id);
            if (existentDartsX01 == null)
            {
                _dartsX01Repository.InsertDartsX01(dartsX01);
                result = "Insert Working!";
            }
            else
            {
                result = $"Darts cricket game exists!";
            }
            return result;
        }
        [HttpPut("id/{id}")]
        public string PutDartsX01(string id, [FromBody] DartsX01 dartsX01)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentDartsX01 = _dartsX01Repository.GetDartsX01(Id);
            dartsX01.Id = id;
            if (existentDartsX01 != null)
            {
                _dartsX01Repository.UpdateDartsX01(dartsX01);
                result = "Update Working!";
            }
            else
            {
                result = $"Darts X01 game don't exists!";
            }
            return result;
        }
        [HttpDelete("id/{id}")]
        public string DeleteDartsX01(string id)
        {
            ObjectId Id = new ObjectId(id);
            var result = string.Empty;
            var existentDartsX01 = _dartsX01Repository.GetDartsX01(Id);

            if (existentDartsX01 != null)
            {
                _dartsX01Repository.RemoveDartsX01(id);
                result = "Delete Working!";
            }
            else
            {
                result = $"Darts X01 game don't exists!";
            }
            return result;
        }
    }
}
