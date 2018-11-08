using GameRoomApp.classes;
using GameRoomApp.providers;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace GameRoomApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ///////-----------------------Test Provider Player
            PlayerProvider playerProvider = new PlayerProvider();
            //Test Add
            Player player = new Player();
            ObjectId objectId = new ObjectId("5be4927ce1ac0307bc508ce2");
            player.Id = objectId;
            player.Name = "Marius";
            player.Age = 23;
            player.Username = "mariusdk";
            player.Password = "suprem";
            player.Score = 100;
            //playerProvider.InsertPlayer(player);

            //Test get by Id
            //Console.WriteLine(playerProvider.GetSpecificPlayer("5be403919208d806f81ba49d").ToString());

            //Test get all
            //Console.WriteLine(playerProvider.GetPlayers());

            //Test update
            //playerProvider.UpdatePlayer(player);

            //Test delete
            //playerProvider.RemovePlayer(player.Id);
            //Console.ReadLine();
            //////////------------------Test provider Team
            ///Add
            Team team = new Team();
            TeamProvider provider = new TeamProvider();
            //team.Name = "Real";

            List<Player> list = new List<Player>();
            //list.Add(player);
            //team.Players = list;
            //provider.InsertTeam(team);
            ///Get One 
            //Console.WriteLine(provider.GetSpecificTeam("5be4936172c23146d8e14148"));
            ///Get All
            //Console.WriteLine(provider.GetAllTeams());
            ///Update 
            //team.Id = new ObjectId("5be4936172c23146d8e14148");
            //team.Name = "Barca";
            //team.Players = list;

            //provider.UpdateTeam(team);
            ///Add player to team
            //provider.AddPlayerToTeam("5be4936172c23146d8e14148", player);
            ///Remove team
            //provider.RemoveTeam("5be4936172c23146d8e14148");

            Console.ReadLine();

        }
    }
}
