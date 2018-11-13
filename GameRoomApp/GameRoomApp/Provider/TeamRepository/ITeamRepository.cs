using GameRoomApp.DataModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers.TeamRepository
{
    public interface ITeamRepository
    {
        void InsertTeam(Team a);
        Team GetTeam(ObjectId Id);
        IEnumerable<Team> GetAllTeams();
        void UpdateTeam(Team team);
        void RemoveTeam(ObjectId Id);
        void AddPlayerToTeam(ObjectId id, Player player);
    }
}
