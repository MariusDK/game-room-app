using GameRoomApp.classes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoomApp.providers
{
    public class TeamProvider
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Team> collection;

        public TeamProvider()
        {
            client = new MongoClient();
            database = client.GetDatabase("gameRoom");
            collection = database.GetCollection<Team>("teams");
        }
        public void InsertTeam(Team a)
        {
            collection.InsertOne(a);
        }
        public Team GetSpecificTeam(ObjectId Id)
        {
           
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Eq("Id",Id);
            var cursor = collection.Find(idFilter);
            Team team = cursor.FirstOrDefault();
            return team;
        }
        public List<Team> GetAllTeams()
        {
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Empty;
            var cursor = collection.Find(idFilter);
            List<Team> teams = cursor.ToList();
            return teams;
        }
        public void UpdateTeam(Team team)
        {
            FilterDefinitionBuilder<Team> Fbuilder = Builders<Team>.Filter;
            var UBuilder = Builders<Team>.Update;
            var idFilter = Fbuilder.Eq("Id", team.Id);
            var updateDefinition = UBuilder.Set("Name", team.Name).Set("Players", team.Players).Set("Game",team.Game);
            var cursor = collection.UpdateOne(idFilter,updateDefinition);
        }
        public void RemoveTeam(ObjectId Id)
        {
            
            var builder = Builders<Team>.Filter;
            var idFilter = builder.Eq("Id",Id);
            collection.DeleteOne(idFilter);
        }
        public void AddPlayerToTeam(ObjectId id,Player player)
        {
            Team team = GetSpecificTeam(id);
            List <Player> players= team.Players;
            players.Add(player);
            team.Players = players;
            UpdateTeam(team);
        }

    }
}
