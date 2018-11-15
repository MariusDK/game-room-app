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
        public void InsertScoreForAllPlayer(Game game)
        {
            List<Player> players = game.Players;
            foreach (Player player in players)
            {
                Score score = new Score(player, game);
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
        public IEnumerable<Score> GetScoresForGame(Game game)
        {
            var builder = Builders<Score>.Filter;
            var gameFilter = builder.Eq("Game", game);
            var cursor = _scoreContext.Score.Find(gameFilter);
            List<Score> scores = cursor.ToList();
            return scores;
        }

        public IEnumerable<Score> LeaderboardForGame(Game game)
        {
            List<Player> players = game.Players;
            List<Score> scores = new List<Score>();
            foreach (Player player in players)
            {
                Score score = GetScoreForPlayer(player, game);
                scores.Add(score);
            }
            LeaderboardSort(scores);
            return scores;

        }
        public Score GetScoreForPlayer(Player player, Game game)
        {
            var builder = Builders<Score>.Filter;
            var gameFilter = builder.Eq("Game", game);
            var playerFilter = builder.Eq("Player", player);
            var filter = gameFilter & playerFilter;
            var cursor = _scoreContext.Score.Find(filter);
            Score score = cursor.FirstOrDefault();
            return score;
        }
        public void LeaderboardSort(List<Score> scores)
        {
            for (int i = 1; i < scores.Count - 1; i++)
            {
                if (scores[i - 1].Value < scores[i].Value)
                {
                    var aux = scores[i - 1];
                    scores[i - 1] = scores[i];
                    scores[i] = aux;
                }
            }
        }
        public void GlobalLeaderboardForGameType(List<Game> games)
        {
            List<Score> scores = new List<Score>();
            List<Player> players = new List<Player>();
            foreach (Game game in games)
            {
                scores = (List<Score>)LeaderboardForGame(game);
                Player winner = scores[0].Player;
                players.Add(winner);
            }
            var leaderboard = new Dictionary<Player, int>();
            foreach (Player player in players)
            {
                int nrWins = GetNumberOfWins(players, player);
                leaderboard.Add(player, nrWins);
            }
        }
        public int GetNumberOfWins(List<Player> players, Player player)
        {
            int count = 0;
            foreach (Player player1 in players)
            {
                if (player.Equals(player1))
                {
                    count++;
                }
            }
            return count;
        }
        public void UpdateScore(Score score)
        {
            FilterDefinitionBuilder<Score> Fbuilder = Builders<Score>.Filter;
            var UBuilder = Builders<Score>.Update;
            var idFilter = Fbuilder.Eq("Id", score.Id);
            var updateDefinition = UBuilder.Set("Player", score.Player).Set("Game", score.Game).
                Set("Value", score.Value);
            var cursor = _scoreContext.Score.UpdateOne(idFilter, updateDefinition);
        }
        public void UpdateScoreByGame(Game newGame,Game oldGame)
        {
            List<Player> players = newGame.Players;
            foreach (Player player in players)
            {
                Score score = GetScoreForPlayer(player, oldGame);
                if (score == null)
                {
                    InsertScoreForAllPlayer(newGame);
                }
                else {
                    score.Game = newGame;
                    UpdateScore(score);
                }
            }

        }
        public void RemoveScore(string Id)
        {
            var builder = Builders<Score>.Filter;
            var idFilter = builder.Eq("Id", Id);
            _scoreContext.Score.DeleteOne(idFilter);
        }
        public void RemoveScoreByGame(Game game)
        {
            List<Player> players = game.Players;
            foreach (Player player in players)
            {
                Score score = GetScoreForPlayer(player, game);
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
    }
}

