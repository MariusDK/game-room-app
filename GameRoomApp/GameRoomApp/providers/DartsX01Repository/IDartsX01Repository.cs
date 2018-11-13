using GameRoomApp.classes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsX01Repository
{
    interface IDartsX01Repository
    {
        void InsertDartsX01(DartsX01 dartsX01);
        DartsX01 GetDartsX01(ObjectId Id);
        IEnumerable<DartsX01> GetAllDartsX01();
        DartsX01 GetDartsX01ByScore(Score score);
        void UpdateDartsX01(DartsX01 dartsX01);
        Dictionary<Player, int> LeaderboardInGameDartsX01(Game game, List<Score> scores);
    }
}
