using GameRoomApp.DataModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.DartsX01Repository
{
    public interface IDartsX01Repository
    {
        void InsertDartsX01(DartsX01 dartsX01);
        DartsX01 GetDartsX01(ObjectId id);
        IEnumerable<DartsX01> GetAllDartsX01();
        DartsX01 GetDartsX01ByScore(Score score);
        void UpdateDartsX01(DartsX01 dartsX01);
        Dictionary<Team, int> LeaderboardInGameDartsX01(Game game, List<Score> scores);
        void RemoveDartsX01(string id);
    }
}
