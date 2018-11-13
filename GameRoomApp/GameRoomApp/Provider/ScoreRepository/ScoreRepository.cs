using GameRoomApp.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace GameRoomApp.providers.ScoreRepository
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly IScoreContext _scoreContext;

        public ScoreRepository(IScoreContext scoreContext)
        {
            this._scoreContext = scoreContext;
        }

        public void InsertScore(Score score)
        {
            _scoreContext.Score.InsertOne(score);
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
        public Score GetScoreForPlayer(Player player,Game game)
        {
            var builder = Builders<Score>.Filter;
            var gameFilter = builder.Eq("Game", game);
            var playerFilter = builder.Eq("Player",player);
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
                int nrWins = GetNumberOfWins(players,player);
                leaderboard.Add(player, nrWins);
            }
        }
        public int GetNumberOfWins(List<Player> players,Player player)
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
        public void RemoveScore(ObjectId Id)
        {
            var builder = Builders<Score>.Filter;
            var idFilter = builder.Eq("Id", Id);
            _scoreContext.Score.DeleteOne(idFilter);
        }

    }
}
