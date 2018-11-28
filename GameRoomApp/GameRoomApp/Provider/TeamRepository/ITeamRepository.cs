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
        Team GetTeam(ObjectId id);
        Team GetTeamByName(string name);
        IEnumerable<Team> GetAllTeams();
        void UpdateTeam(Team team);
        void UpdateTeamByName(string name,Team team);
        void RemoveTeam(ObjectId id);
        void RemoveTeamByName(string name);
        void AddPlayerToTeam(ObjectId id, Player player);
        void RemovePlayerFromTeam(ObjectId id, string idPlayer);
    }
}
