using GameRoomApp.DataModel;
using GameRoomApp.providers.DartsCricketRepository;
using GameRoomApp.providers.DartsX01Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace GameRoomApp.providers.ScoreRepository
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly IScoreContext _scoreContext;
        private readonly IDartsCricketRepository _dartsCricketRepository;
        private readonly IDartsX01Repository _dartsX01Repository;

        public ScoreRepository(IScoreContext scoreContext, IDartsCricketRepository dartsCricketRepository,
            IDartsX01Repository dartsX01Repository)
        {
            this._scoreContext = scoreContext;
            this._dartsCricketRepository = dartsCricketRepository;
            this._dartsX01Repository = dartsX01Repository;
        }


        public void InsertScore(Score score)
        {
            _scoreContext.Score.InsertOne(score);
        }
        public void InsertScoreForAllPlayers(Game game)
        {
            List<Team> teams = game.Teams;
            foreach (Team team in teams)
            {
                Score score = new Score(team, game);
                InsertScore(score);
                if (game.Type.Equals("Darts/Cricket"))
                {
                    DartsCricket dartsCricket = new DartsCricket(score);
                    _dartsCricketRepository.InsertDartsCricket(dartsCricket);
                }
                if (game.Type.Equals("Darts/X01"))
                {
                    DartsX01 dartsX01 = new DartsX01(score);
                    _dartsX01Repository.InsertDartsX01(dartsX01);
                }
            }
        }
        public List<Score> GetScoresForGame(Game game)
        {
            var builder = Builders<Score>.Filter;
            var gameFilter = builder.Eq("Game.Id", game.Id);
            var cursor = _scoreContext.Score.Find(gameFilter);
            List<Score> scores = cursor.ToList();
            return scores;
        }

        public IEnumerable<Score> LeaderboardForGame(Game game)
        {
            List<Team> teams = game.Teams;
            List<Score> scores = new List<Score>();
            foreach (Team team in teams)
            {
                Score score = GetScoreForTeam(team, game);
                scores.Add(score);
            }
            scores = LeaderboardSort(scores);
            return scores;

        }
        public Score GetScoreForTeam(Team team, Game game)
        {
            var builder = Builders<Score>.Filter;
            var gameFilter = builder.Eq("Game.Id", game.Id);
            var teamFilter = builder.Eq("Team.Id", team.Id);
            var filter = gameFilter & teamFilter;
            var cursor = _scoreContext.Score.Find(filter);
            Score score = cursor.FirstOrDefault();
            return score;
        }
        public List<Score> LeaderboardSort(List<Score> scores)
        {
            for (int i = 0; i < scores.Count - 1; i++)
            {
                for (int j = i+1; j < scores.Count; j++)
                {
                    if (scores[i].Value < scores[j].Value)
                    {
                        var aux = scores[i];
                        scores[i] = scores[j];
                        scores[j] = aux;
                    }
                }
            }
            return scores;
        }
        public void GlobalLeaderboardForGameType(List<Game> games)
        {
            List<Score> scores = new List<Score>();
            List<Team> teams = new List<Team>();
            foreach (Game game in games)
            {
                scores = (List<Score>)LeaderboardForGame(game);
                Team winner = scores[0].Team;
                teams.Add(winner);
            }
            var leaderboard = new Dictionary<Team, int>();
            foreach (Team team in teams)
            {
                int nrWins = GetNumberOfWins(teams, team);
                leaderboard.Add(team, nrWins);
            }
        }
        public int GetNumberOfWins(List<Team> teams, Team team)
        {
            int count = 0;
            foreach (Team team1 in teams)
            {
                if (team.Equals(team1))
                {
                    count++;
                }
            }
            return count;
        }
        public void UpdateScore(Score score)
        {
            FilterDefinitionBuilder<Score> fBuilder = Builders<Score>.Filter;
            var uBuilder = Builders<Score>.Update;
            var idFilter = fBuilder.Eq("Id", score.Id);
            var updateDefinition = uBuilder.Set("Team", score.Team).Set("Game", score.Game).
                Set("Value", score.Value);
            var cursor = _scoreContext.Score.UpdateOne(idFilter, updateDefinition);
        }
        public void UpdateScoreByGame(Game newGame,Game oldGame)
        {
            List<Team> teams = newGame.Teams;
            foreach (Team team in teams)
            {
                Score score = GetScoreForTeam(team, oldGame);
                if (score == null)
                {
                    InsertScoreForAllPlayers(newGame);
                }
                else {
                    score.Game = newGame;
                    UpdateScore(score);
                }
            }

        }
        public void RemoveScore(string id)
        {
            var builder = Builders<Score>.Filter;
            var idFilter = builder.Eq("Id", id);
            _scoreContext.Score.DeleteOne(idFilter);
        }
        public void RemoveScoreByGame(Game game)
        {
            List<Team> teams = game.Teams;
            foreach (Team team in teams)
            {
                Score score = GetScoreForTeam(team, game);
                if (score != null)
                {
                    RemoveScore(score.Id);
                    if (game.Type.Equals("Darts/Cricket"))
                    {
                        DartsCricket dartsCricket = _dartsCricketRepository.GetDartsCricketByScore(score);
                        _dartsCricketRepository.RemoveDartsCricket(dartsCricket.Id);
                    }
                    if (game.Type.Equals("Darts/X01"))
                    {
                        DartsX01 dartsX01 = _dartsX01Repository.GetDartsX01ByScore(score);
                        _dartsX01Repository.RemoveDartsX01(dartsX01.Id);
                    }
                }
            }
        }

        public Score GetScoreById(string id)
        {
            var builder = Builders<Score>.Filter;
            var idFilter = builder.Eq("Id", id);
            var cursor = _scoreContext.Score.Find(idFilter);
            Score score = cursor.FirstOrDefault();
            return score;
        }

        public void RemoveScoresOfGames(List<Game> games)
        {
            foreach (Game game in games)
            {
                RemoveScoreByGame(game);
            }
        }
        public void UpdatePlayerScores(List<Game> games)
        {
            List<Score> scores = new List<Score>();
            foreach (Game game in games)
            {
                foreach (Team team in game.Teams)
                {
                    Score score = GetScoreForTeam(team, game);
                    score.Team = team;
                    score.Game = game;
                    UpdateScore(score);
                    if (game.Type.Equals("Darts/X01"))
                    {
                        DartsX01 dartsX01 = _dartsX01Repository.GetDartsX01ByScore(score);
                        dartsX01.Score = score;
                        _dartsX01Repository.UpdateDartsX01(dartsX01);
                    }
                    if (game.Type.Equals("Darts/Cricket"))
                    {
                        DartsCricket dartsCricket = _dartsCricketRepository.GetDartsCricketByScore(score);
                        dartsCricket.Score = score;
                        _dartsCricketRepository.UpdateDartsCricket(dartsCricket);
                    }
                }   
            }
        }
    }
}

