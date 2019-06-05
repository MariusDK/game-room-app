using GameRoomApp.DataModel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameRoomApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!DatabaseExists("gameRoom"))
            {
                List<Player> players = SeedPlayers();
                List<Team> teams = SeedTeams(players);
                SeedGames(teams);
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static bool DatabaseExists(string database)
        {
            IMongoClient _client = new MongoClient();
            var dbList = _client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
            return dbList.Contains(database);
        }
        public static List<Player> SeedPlayers()
        { 
            MongoClient client = new MongoClient();   
            IMongoDatabase mongoDatabase = client.GetDatabase("gameRoom");    
            IMongoCollection<Player> playerCol = mongoDatabase.GetCollection<Player>("Player");
            List<Player> playerList = new List<Player>();
            Player player1 = new Player("5bec01f13b4a4a3cd81f456a", "Andrei Marian", "andreiM", "123456", 20);
            Player player2 = new Player("5bec01f13b4a4a3cd81f456b", "Mihai Andrea", "mAndrea", "000000", 30);
            Player player3 = new Player("5bec01f13b4a4a3cd81f456c", "Noris Rad", "noritsRad", "999999", 25);
            Player player4 = new Player("5bec01f13b4a4a3cd81f456d", "Gicu Groza", "gicuggg", "172834", 28);
            Player player5 = new Player("5bec01f13b4a4a3cd81f456e", "Aurel Mita", "mitaurel", "986344", 40);
            Player player6 = new Player("5bec01f13b4a4a3cd81f4566", "Admin123", "admin123", "0192023A7BBD73250516F069DF18B500", 25);
            playerList.Add(player1);
            playerList.Add(player2);
            playerList.Add(player3);
            playerList.Add(player4);
            playerList.Add(player5);
            playerList.Add(player6);
            playerCol.InsertMany(playerList);
            return playerList;
        }
        public static List<Team> SeedTeams(List<Player> players)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase mongoDatabase = client.GetDatabase("gameRoom");
            IMongoCollection<Team> teamCol = mongoDatabase.GetCollection<Team>("Team");

            List<Player> players1 = new List<Player>();
            List<Player> players2 = new List<Player>();
            List<Player> players3 = new List<Player>();
            List<Player> players4 = new List<Player>();
            List<Player> players5 = new List<Player>();
            List<Player> players6 = new List<Player>();
            List<Player> players7 = new List<Player>();
            players1.Add(players[0]);
            players1.Add(players[2]);
            
            players2.Add(players[3]);
            players2.Add(players[4]);

            players3.Add(players[0]);
            players4.Add(players[3]);
            players5.Add(players[1]);

            players6.Add(players[5]);
            players7.Add(players[1]);
            players7.Add(players[5]);
            Team team1 = new Team("e746bc2083d589e0c3f879d5", "Fire", players1);
            Team team2 = new Team("64259fe6ad8be15ab6a4ed84", "Air", players2);
            Team team3 = new Team("63556a815f8f2b8792396f59","",players3);
            Team team4 = new Team("cd4207c1b87e5c9e5bb96289", "", players4);
            Team team5 = new Team("8cbc3d7f52393ababfa42f67", "", players5);
            Team team6 = new Team("8cbc3d7f52393ababfa42f62", "", players6);
            Team team7 = new Team("8cbc3d7f52393ababfa42f63", "Phonex", players7);
            List<Team> teams = new List<Team>();
            teams.Add(team1);
            teams.Add(team2);
            teams.Add(team3);
            teams.Add(team4);
            teams.Add(team5);
            teams.Add(team6);
            teams.Add(team7);
            teamCol.InsertMany(teams);
            return teams;
        }
        public static void SeedGames(List<Team> teams)
        {
            MongoClient client = new MongoClient();
            IMongoDatabase mongoDatabase = client.GetDatabase("gameRoom");
            IMongoCollection<Game> gameCol = mongoDatabase.GetCollection<Game>("Game");
            IMongoCollection<Score> scoreCol = mongoDatabase.GetCollection<Score>("Score");
            IMongoCollection<DartsCricket> dartsCricketCol = mongoDatabase.GetCollection<DartsCricket>("DartsCricket");
            IMongoCollection<DartsX01> dartsX01Col = mongoDatabase.GetCollection<DartsX01>("DartsX01");

            List<Team> teams1 = new List<Team>();
            teams1.Add(teams[0]);
            teams1.Add(teams[6]);

            List<Team> teams2 = new List<Team>();
            teams2.Add(teams[6]);
            teams2.Add(teams[1]);


            List<Team> teams3 = new List<Team>();
            teams3.Add(teams[5]);
            teams3.Add(teams[2]);


            //Game game1 = new Game("5bec01f13b4a4a3cd81f456f", "Joc1", "Fifa", teams1, DateTime.Now);
            //Game game2 = new Game("994f8881011ea98668f4f090", "Joc2", "Darts/Cricket", teams1, DateTime.Now);
            //Game game3 = new Game("df8b4518c1f279c812c3e9c6", "Joc3", "Darts/X01", teams1, DateTime.Now);
            //Game game4 = new Game("92acbcb8d9e3b016f147973b", "Joc4", "Darts/Cricket", teams2, DateTime.Now);
            //Game game5 = new Game("e65909d50566e7da93c18a34", "Joc5", "Darts/X01", teams3, DateTime.Now);
            //Game game6 = new Game("480231d988cc45af532b0465", "Joc6", "Foosball", teams2, DateTime.Now);


            //List<Game> games = new List<Game>();
            //games.Add(game1);
            //games.Add(game2);
            //games.Add(game3);
            //games.Add(game4);
            //games.Add(game5);
            //games.Add(game6);

            //gameCol.InsertMany(games);
            List<Score> scores = new List<Score>();
            List<DartsCricket> dartsCrickets = new List<DartsCricket>();
            List<DartsX01> dartsX01s = new List<DartsX01>();

            //Score score1 = new Score("53a6f7fe7203d6e6a544b65c",teams[0],game1);
            //scores.Add(score1);
            //Score score2 = new Score("a039dae78caf276e5195e6f9", teams[1], game1);
            //scores.Add(score2);

            //Score score3 = new Score("917dd10d4e37a4bab350fdb1", teams[0], game2);
            //scores.Add(score3);
            //DartsCricket dartsCricket1 = new DartsCricket(score3);
            //dartsCrickets.Add(dartsCricket1);
            //Score score4 = new Score("606f58a9d9ae17388095735e", teams[1], game2);
            //scores.Add(score4);
            //DartsCricket dartsCricket2 = new DartsCricket(score4);
            //dartsCrickets.Add(dartsCricket2);

            //Score score5 = new Score("e44af887fa698eba93860929", teams[0], game3);
            //scores.Add(score5);
            //DartsX01 dartsX011 = new DartsX01(score5);
            //dartsX01s.Add(dartsX011);
            //Score score6 = new Score("f9dfc49456e924f75b873bfd", teams[1], game3);
            //scores.Add(score6);
            //DartsX01 dartsX012 = new DartsX01(score6);
            //dartsX01s.Add(dartsX012);

            //Score score7 = new Score("cac8fc99190916d692418dd6", teams[2], game4);
            //scores.Add(score7);
            //DartsCricket dartsCricket3 = new DartsCricket(score7);
            //dartsCrickets.Add(dartsCricket3);
            //Score score8 = new Score("c4c356e866fc99a89723113e", teams[3], game4);
            //scores.Add(score8);
            //DartsCricket dartsCricket4 = new DartsCricket(score8);
            //dartsCrickets.Add(dartsCricket4);

            //Score score9 = new Score("d96a906662e793726bd6f644", teams[2], game5);
            //scores.Add(score9);
            //DartsX01 dartsX013 = new DartsX01(score9);
            //dartsX01s.Add(dartsX013);
            //Score score10 = new Score("dcc57e854a7754536115b626", teams[4], game5);
            //scores.Add(score10);
            //DartsX01 dartsX014 = new DartsX01(score10);
            //dartsX01s.Add(dartsX014);

            //Score score11 = new Score("33a00ae166d60fde092ed876", teams[2], game6);
            //scores.Add(score11);
            //Score score12 = new Score("52a2a2d08b5a6aab9d7953a3", teams[3], game6);
            //scores.Add(score12);

            //scoreCol.InsertMany(scores);
            //dartsCricketCol.InsertMany(dartsCrickets);
            //dartsX01Col.InsertMany(dartsX01s);

        }
    }
}
