﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.DataModel
{
    [BsonIgnoreExtraElements]
    public class DartsX01
    {
        public DartsX01(Score score)
        {
            this.Score = score;
            this.StateScore = 501;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Score Score { get; set; }
        public int StateScore { get; set; }
    }
}
