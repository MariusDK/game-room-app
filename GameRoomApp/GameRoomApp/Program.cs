using GameRoomApp.classes;
using GameRoomApp.providers;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace GameRoomApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
