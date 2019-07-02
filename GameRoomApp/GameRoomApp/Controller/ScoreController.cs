using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameRoomApp.providers.GameRepository;
using GameRoomApp.Util.CART;
using MongoDB.Bson;
using GameRoomApp.providers.PlayerRepository;
using GameRoomApp.providers.TeamRepository;

namespace GameRoomApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        public ScoreController(IScoreRepository scoreRepository,IGameRepository gameRepository,
            IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            this._scoreRepository = scoreRepository;
            this._gameRepository = gameRepository;
            this._playerRepository = playerRepository;
            this._teamRepository = teamRepository;
        }
        [HttpGet]
        [ActionName("GetScore")]
        [ExactQueryParam("scoreId")]
        public Score GetScore(string scoreId)
        {
            return _scoreRepository.GetScoreById(scoreId);
        }
        [HttpGet]
        [ActionName("GetPrediction")]
        [ExactQueryParam("gameId","playerId")]
        public string GetPrediction(string gameId,string playerId)
        {
            List<Score> AllScores = _scoreRepository.GetScoresOfPlayer(playerId);
            List<Score> scores = new List<Score>();

            foreach (Score score in AllScores)
            {
                //de verificat
                if (!(score.Game.Id.Equals(gameId)) && (score.Game.EndOn != null))
                {
                    scores.Add(score);
                }
            }
            List<string> features = new List<string>() { "GameType", "Location", "Opponents", "Age", "Referee" };
            ObjectId gameObId = new ObjectId(gameId);
            Game game = _gameRepository.GetGameById(gameObId);
            Team teamOfPlayer = _gameRepository.GetPlayerTeam(game, playerId);
            List<Player> opponents = new List<Player>();
            foreach (Team team in game.Teams)
            {
                if (!(team.Id.Equals(teamOfPlayer.Id)))
                {
                    foreach (Player opponent in team.Players)
                    {
                        opponents.Add(opponent);
                    }
                }
            }
            Player player = _playerRepository.GetPlayerById(new ObjectId(playerId));
            DecisionTree decisionTree = new DecisionTree(scores,features,opponents,player,game,_scoreRepository);
            return decisionTree.prediction;
        }
        [HttpGet]
        [ActionName("GetPredictionTeam")]
        [ExactQueryParam("gameId", "teamId")]
        public string GetPredictionTeam(string gameId, string teamId)
        {
      
            List<Score> AllScores = _scoreRepository.GetScoresOfTeam(teamId);
            List<Score> scores = new List<Score>();

            foreach (Score score in AllScores)
            {
                //de verificat
                if (!(score.Game.Id.Equals(gameId)) && (score.Game.EndOn != null))
                {
                    scores.Add(score);
                }
            }
            List<string> features = new List<string>() { "GameType", "Location", "Opponents", "Age", "Referee" };
            ObjectId gameObId = new ObjectId(gameId);
            Game game = _gameRepository.GetGameById(gameObId);
            ObjectId teamObId = new ObjectId(teamId);
            Team currentTeam = _teamRepository.GetTeam(teamObId);
            List<Team> opponents = new List<Team>();
            foreach (Team team in game.Teams)
            {
                if (!(team.Id.Equals(currentTeam.Id)))
                {
                    opponents.Add(team);   
                }
            }
            
            DecisionTreeTeam decisionTree = new DecisionTreeTeam(scores, features, opponents, currentTeam, game, _scoreRepository);
            return decisionTree.prediction;
        }
        [HttpGet]
        [ActionName("GetPredictionProcent")]
        [ExactQueryParam("gameId", "playerId","regression")]
        public double GetPredictionProcent(string gameId, string playerId,string regression)
        {
            List<Score> AllScores = _scoreRepository.GetScoresOfPlayer(playerId);
            List<Score> scores = new List<Score>();
            Score currentScore = new Score();
            foreach (Score score in AllScores)
            {
                if ((!(score.Game.Id.Equals(gameId)))&&(score.Game.EndOn!=null))
                {
                   
                    scores.Add(score);
                }
                else
                {
                    if (score.Game.Id.Equals(gameId))
                    {
                        currentScore = score;
                    }
                }
            }
            List<string> features = new List<string>() { "GameType", "Location", "Opponents", "Age", "Referee" };
            ObjectId gameObId = new ObjectId(gameId);
            Game game = _gameRepository.GetGameById(gameObId);
            Team teamOfPlayer = _gameRepository.GetPlayerTeam(game, playerId);
            List<Player> opponents = new List<Player>();
            foreach (Team team in game.Teams)
            {
                if (!(team.Id.Equals(teamOfPlayer.Id)))
                {
                    foreach (Player opponent in team.Players)
                    {
                        opponents.Add(opponent);
                    }
                }
            }
            Player player = _playerRepository.GetPlayerById(new ObjectId(playerId));
            DecisionTree decisionTreeMain = new DecisionTree(scores, features, opponents, player, game, _scoreRepository,"regression");
            //facem sansa fiecarui oponent
            foreach (Player opponent in opponents)
            {
                List<Player> opponentsO = new List<Player>();
                List<Score> opponentAllScores = _scoreRepository.GetScoresOfPlayer(opponent.Id);
                List<Score> opponentScores = new List<Score>();
                Score currentOpponentScore = new Score();
                foreach (Score score in opponentAllScores)
                {
                    if ((!(score.Game.Id.Equals(gameId))) && (score.Game.EndOn != null))
                    {
                        opponentScores.Add(score);
                    }
                    else
                    {
                        if (score.Game.Id.Equals(gameId))
                        {
                            currentOpponentScore = score;
                        }
                    }
                }
                Team teamOfOpponent = _gameRepository.GetPlayerTeam(game, opponent.Id);
                foreach (Team team in game.Teams)
                {
                    if (!(team.Id.Equals(teamOfOpponent.Id)))
                    {
                        foreach (Player opponenti in team.Players)
                        {
                            opponentsO.Add(opponenti);
                        }
                    }
                }
                DecisionTree decisionTree = new DecisionTree(opponentScores, features, opponentsO, opponent, game, _scoreRepository, "regression");
                double procentualPredictionOpponent = decisionTree.procentualPrediction;
                currentOpponentScore.ChanceOfVictory = procentualPredictionOpponent;
                _scoreRepository.UpdateScore(currentOpponentScore);
            }
            double procentualPrediction = decisionTreeMain.procentualPrediction;
            currentScore.ChanceOfVictory = procentualPrediction;
            _scoreRepository.UpdateScore(currentScore);
            return procentualPrediction;
        }
        [HttpGet]
        [ActionName("GetPredictionProcentTeam")]
        [ExactQueryParam("gameId", "teamId", "regression")]
        public double GetPredictionProcentTeam(string gameId, string teamId, string regression)
        {
            List<Score> AllScores = _scoreRepository.GetScoresOfTeam(teamId);
            List<Score> scores = new List<Score>();
            Score currentScore = new Score();
            foreach (Score score in AllScores)
            {
                if ((!(score.Game.Id.Equals(gameId))) && (score.Game.EndOn != null))
                {

                    scores.Add(score);
                }
                else
                {
                    if (score.Game.Id.Equals(gameId))
                    {
                        currentScore = score;
                    }
                }
            }
            List<string> features = new List<string>() { "GameType", "Location", "Opponents", "Age", "Referee" };
            ObjectId gameObId = new ObjectId(gameId);
            Game game = _gameRepository.GetGameById(gameObId);
            ObjectId teamIdObj= new ObjectId(teamId);
            Team teamOfPlayer = _teamRepository.GetTeam(teamIdObj);
            List<Team> opponents = new List<Team>();
            foreach (Team team in game.Teams)
            {
                if (!(team.Id.Equals(teamOfPlayer.Id)))
                {
                    opponents.Add(team);   
                }
            }
            DecisionTreeTeam decisionTreeMain = new DecisionTreeTeam(scores, features, opponents, teamOfPlayer, game, _scoreRepository, "regression");
            //facem sansa fiecarui oponent
            foreach (Team opponent in opponents)
            {
                List<Score> opponentAllScores = _scoreRepository.GetScoresOfTeam(opponent.Id);
                List<Score> opponentScores = new List<Score>();
                Score currentOpponentScore = new Score();
                foreach (Score score in opponentAllScores)
                {
                    if ((!(score.Game.Id.Equals(gameId))) && (score.Game.EndOn != null))
                    {
                        opponentScores.Add(score);
                    }
                    else
                    {
                        if (score.Game.Id.Equals(gameId))
                        {
                            currentOpponentScore = score;
                        }
                    }
                }
                List<Team> opponentsO = new List<Team>();
                foreach (Team team in game.Teams)
                {
                    if (!(team.Id.Equals(opponent.Id)))
                    { 
                        opponentsO.Add(opponent);
                    }
                }
                DecisionTreeTeam decisionTree = new DecisionTreeTeam(opponentScores, features, opponentsO, opponent, game, _scoreRepository, "regression");
                double procentualPredictionOpponent = decisionTree.procentualPrediction;
                currentOpponentScore.ChanceOfVictory = procentualPredictionOpponent;
                _scoreRepository.UpdateScore(currentOpponentScore);
            }
            double procentualPrediction = decisionTreeMain.procentualPrediction;
            currentScore.ChanceOfVictory = procentualPrediction;
            _scoreRepository.UpdateScore(currentScore);
            return procentualPrediction;
        }
        [HttpGet]
        [ActionName("GetScoresOfGame")]
        [ExactQueryParam("gameName")]
        public IEnumerable<Score> GetScoresOfGame(string gameName)
        {
           
            Game game = _gameRepository.GetGameByName(gameName);
            return _scoreRepository.GetScoresForGame(game);
       
        }

        [HttpPut]
        [ExactQueryParam("scoreId")]
        public string Put(string scoreId, [FromBody] Score score)
        {

            var result = string.Empty;
            var existentScore = _scoreRepository.GetScoreById(scoreId);

            if (existentScore != null)
            {

                _scoreRepository.UpdateScore(score);
                result = "Update Working!";
            }
            else
            {
                result = $"Score don't exists!";
            }
            return result;
        }
        [HttpDelete]
        [ExactQueryParam("scoreId")]
        public string DeleteScore(string scoreId)
        {
            var result = string.Empty;
            var existentScore = _scoreRepository.GetScoreById(scoreId);

            if (existentScore != null)
            {
                _scoreRepository.RemoveScore(scoreId);
                result = "Delete Working!";
            }
            else
            {
                result = $"Score don't exists!";
            }
            return result;
        }
    }
}
