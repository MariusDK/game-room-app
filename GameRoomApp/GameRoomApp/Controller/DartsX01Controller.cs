﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.DartsX01Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GameRoomApp.Controller
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
        [HttpGet]
        [ActionName(nameof(GetDartsX01))]
        [ExactQueryParam("dartsId")]
        public DartsX01 GetDartsX01(string dartsId)
        {
            ObjectId idObject = new ObjectId(dartsId);
            return _dartsX01Repository.GetDartsX01(idObject);
        }
        [HttpPost]
        public string PostDartsX01([FromBody] DartsX01 dartsX01)
        {
            var result = string.Empty;
            ObjectId id = new ObjectId(dartsX01.Id);
            var existentDartsX01 = _dartsX01Repository.GetDartsX01(id);
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
        [HttpPut]
        [ExactQueryParam("dartsId")]
        public string PutDartsX01(string dartsId, [FromBody] DartsX01 dartsX01)
        {
            ObjectId idObject = new ObjectId(dartsId);
            var result = string.Empty;
            var existentDartsX01 = _dartsX01Repository.GetDartsX01(idObject);
            dartsX01.Id = dartsId;
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
        [HttpDelete]
        [ExactQueryParam("dartsId")]
        public string DeleteDartsX01(string dartsId)
        {
            ObjectId idObject = new ObjectId(dartsId);
            var result = string.Empty;
            var existentDartsX01 = _dartsX01Repository.GetDartsX01(idObject);

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
