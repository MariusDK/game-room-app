using GameRoomApp.DataModel;
using GameRoomApp.providers;
using GameRoomApp.providers.PlayerRepository;
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
                GenerateDatabase();
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
        public static void GenerateDatabase()
        {
            MongoClient client = new MongoClient();
            IMongoDatabase mongoDatabase = client.GetDatabase("gameRoom");
            IMongoCollection<Player> playerCol = mongoDatabase.GetCollection<Player>("Player");
            IMongoCollection<Game> gameCol = mongoDatabase.GetCollection<Game>("Game");
            IMongoCollection<Team> teamCol = mongoDatabase.GetCollection<Team>("Team");
            IMongoCollection<Score> scoreCol = mongoDatabase.GetCollection<Score>("Score");
            IMongoCollection<DartsCricket> dartsCricketCol = mongoDatabase.GetCollection<DartsCricket>("DartsCricket");
            IMongoCollection<DartsX01> dartsX01Col = mongoDatabase.GetCollection<DartsX01>("DartsX01");

            Player player1 = new Player("5bec01f13b4a4a3cd81f456a", "Andrei Marian", "andreiM", "123456", 20);
            Player player2 = new Player("5bec01f13b4a4a3cd81f456b", "Mihai Andrea", "mAndrea", "000000", 30);
            Player player3 = new Player("5bec01f13b4a4a3cd81f456c", "Noris Rad", "noritsRad", "999999", 25);
            Player player4 = new Player("5bec01f13b4a4a3cd81f456d", "Gicu Groza", "gicuggg", "172834", 28);
            Player player5 = new Player("5bec01f13b4a4a3cd81f456e", "Aurel Mita", "mitaurel", "986344", 40);

            playerCol.InsertOne(player1);
            playerCol.InsertOne(player2);
            playerCol.InsertOne(player3);
            playerCol.InsertOne(player4);
            playerCol.InsertOne(player5);

            List<Player> players1 = new List<Player>();
            players1.Add(player1);
            players1.Add(player3);
            players1.Add(player5);

            List<Player> players2 = new List<Player>();
            players2.Add(player2);
            players2.Add(player4);
            players2.Add(player5);

            List<Player> players3 = new List<Player>();
            players2.Add(player1);
            players2.Add(player2);
            players2.Add(player4);

            Game game1 = new Game("5bec01f13b4a4a3cd81f456f", "Joc1", "Fifa", players1);
            Game game2 = new Game("994f8881011ea98668f4f090", "Joc2", "Darts/Cricket", players1);
            Game game3 = new Game("df8b4518c1f279c812c3e9c6", "Joc3", "Darts/X01", players1);
            Game game4 = new Game("92acbcb8d9e3b016f147973b", "Joc4", "Darts/Cricket", players2);
            Game game5 = new Game("e65909d50566e7da93c18a34", "Joc5", "Darts/X01", players2);
            Game game6 = new Game("480231d988cc45af532b0465", "Joc6", "Foosball", players3);

            gameCol.InsertOne(game1);
            gameCol.InsertOne(game2);
            gameCol.InsertOne(game3);
            gameCol.InsertOne(game4);
            gameCol.InsertOne(game5);
            gameCol.InsertOne(game6);

            Score score1 = new Score(player1, game1);
            Score score2 = new Score(player3, game1);
            Score score3 = new Score(player5, game1);

            scoreCol.InsertOne(score1);
            scoreCol.InsertOne(score2);
            scoreCol.InsertOne(score3);

            Score score4 = new Score("f02785b16d8d5c567b0d0b21", player1, game2);
            DartsCricket dartsCricket1 = new DartsCricket(score4);
            Score score5 = new Score("e81f7af2971a9e3cf9baa2a3", player3, game2);
            DartsCricket dartsCricket2 = new DartsCricket(score5);
            Score score6 = new Score("f1fd02e940bf041c3c613e4e", player5, game2);
            DartsCricket dartsCricket3 = new DartsCricket(score6);

            scoreCol.InsertOne(score4);
            scoreCol.InsertOne(score5);
            scoreCol.InsertOne(score6);
            dartsCricketCol.InsertOne(dartsCricket1);
            dartsCricketCol.InsertOne(dartsCricket2);
            dartsCricketCol.InsertOne(dartsCricket3);

            Score score7 = new Score("c031f962f4782a69c1928f0f", player1, game3);
            DartsX01 dartsX011 = new DartsX01(score7);
            Score score8 = new Score("fa72b3eaac0910eb73ce0fc6", player3, game3);
            DartsX01 dartsX012 = new DartsX01(score8);
            Score score9 = new Score("2f27f16ae307d1a2e1fc025c", player5, game3);
            DartsX01 dartsX013 = new DartsX01(score9);

            scoreCol.InsertOne(score7);
            scoreCol.InsertOne(score8);
            scoreCol.InsertOne(score9);
            dartsX01Col.InsertOne(dartsX011);
            dartsX01Col.InsertOne(dartsX012);
            dartsX01Col.InsertOne(dartsX013);

            Score score10 = new Score("98dc1188f7ea24cf2bb28a38", player2, game4);
            DartsCricket dartsCricket4 = new DartsCricket(score10);
            Score score11 = new Score("790dc716dc076918629bc0d3", player4, game4);
            DartsCricket dartsCricket5 = new DartsCricket(score11);
            Score score12 = new Score("58700907d483abdfa78cd959", player5, game4);
            DartsCricket dartsCricket6 = new DartsCricket(score12);

            scoreCol.InsertOne(score10);
            scoreCol.InsertOne(score11);
            scoreCol.InsertOne(score12);
            dartsCricketCol.InsertOne(dartsCricket4);
            dartsCricketCol.InsertOne(dartsCricket5);
            dartsCricketCol.InsertOne(dartsCricket6);

            Score score13 = new Score("8a604e6bd9940cff639a5077", player2, game5);
            DartsX01 dartsX014 = new DartsX01(score13);
            Score score14 = new Score("d08a0f8e89e1290a09e7a893", player4, game5);
            DartsX01 dartsX015 = new DartsX01(score14);
            Score score15 = new Score("9ee4c0cc347302f5c0fad0bc", player5, game5);
            DartsX01 dartsX016 = new DartsX01(score15);

            scoreCol.InsertOne(score13);
            scoreCol.InsertOne(score14);
            scoreCol.InsertOne(score15);
            dartsX01Col.InsertOne(dartsX014);
            dartsX01Col.InsertOne(dartsX015);
            dartsX01Col.InsertOne(dartsX016);

            Score score16 = new Score(player1, game6);
            Score score17 = new Score(player2, game6);
            Score score18 = new Score(player4, game6);

            scoreCol.InsertOne(score16);
            scoreCol.InsertOne(score17);
            scoreCol.InsertOne(score18);

            List<Player> players4 = new List<Player>();
            players4.Add(player1);
            players4.Add(player3);
            players4.Add(player2);
            players4.Add(player4);

            Game game7 = new Game("3ca9cd88b2f2580351e1ab5f", "Joc7", "Fifa", players4);
            gameCol.InsertOne(game7);

            List<Player> players5 = new List<Player>();
            players5.Add(player1);
            players5.Add(player3);
            List<Player> players6 = new List<Player>();
            players6.Add(player2);
            players6.Add(player4);

            Team team1 = new Team("Fire", players5, game7);
            Team team2 = new Team("Air", players6, game7);
            teamCol.InsertOne(team1);
            teamCol.InsertOne(team2);
        }
    }
}
