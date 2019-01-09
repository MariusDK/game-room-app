using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GameRoomApp.providers;
using GameRoomApp.providers.DartsCricketRepository;
using GameRoomApp.providers.DartsX01Repository;
using GameRoomApp.providers.GameRepository;
using GameRoomApp.providers.PlayerRepository;
using GameRoomApp.providers.ScoreRepository;
using GameRoomApp.providers.TeamRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace GameRoomApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<Settings>(settings =>
            {
                settings.ConnectionString = Configuration.GetSection("MongoDB:ConnectionString").Value;
                settings.DatabaseName = Configuration.GetSection("MongoDB:Database").Value;
            });
            services.AddTransient<IPlayerContext, PlayerContext>();
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<ITeamContext, TeamContext>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IGameContext, GameContext>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IScoreContext, ScoreContext>();
            services.AddTransient<IScoreRepository, ScoreRepository>();
            services.AddTransient<IDartsX01Context, DartsX01Context>();
            services.AddTransient<IDartsX01Repository, DartsX01Repository>();
            services.AddTransient<IDartsCricketContext, DartsCricketContext>();
            services.AddTransient<IDartsCricketRepository, DartsCricketRepository>();


            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMvc();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
