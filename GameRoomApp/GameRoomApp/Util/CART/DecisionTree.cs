using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameRoomApp.Util.CART
{
    public class DecisionTree
    {
        public Node root { get; set; }
        public string prediction { get; set; }
        public IScoreRepository scoreRepository;

        public DecisionTree(Node root, IScoreRepository scoreRepository)
        {
            this.prediction = null;
            this.root = root;
            this.scoreRepository = scoreRepository;
        }
        public DecisionTree(List<Score> scores, List<string> features, List<Player> opponents, Player player, Game game,IScoreRepository scoreRepository)
        {
            this.scoreRepository = scoreRepository;
            string bestFeature = GetBestFeature(features, scores, opponents, player);
            features.Remove(bestFeature);
            this.root = new Node();
            this.root.value = bestFeature;
            this.root.left = new Node();
            this.root.right = new Node();
            scores = GetScoresAfterFeature(game, bestFeature, scores, opponents);
            Node currentNode = root;
            Queue<Node> nodes = new Queue<Node>();
            nodes.Enqueue(root.left);
            nodes.Enqueue(root.right);
            while (features.Count != 0)
            {
                if (bestFeature.Equals("empty"))
                {
                    break;
                }
                if (scores.Count<2)
                {
                    break;
                }
                if (nodes.Count != 0)
                {
                    currentNode = nodes.First();
                    bestFeature = GetBestFeature(features, scores, opponents, player);
                    features.Remove(bestFeature);
                    scores = GetScoresAfterFeature(game, bestFeature, scores, opponents);
                    currentNode.left = new Node();
                    currentNode.right = new Node();
                    nodes.Enqueue(currentNode.left);
                    nodes.Enqueue(currentNode.right);
                }
            }
            if (scores.Count != 0)
            {
                prediction = MakeDecision(scores);
            }
            else {
                prediction = "no decision";
            }

        }
        public List<Score> GetScoresAfterFeature(Game game, string feature, List<Score> scores, List<Player> opponents)
        {
            List<Score> remainingScore = new List<Score>();

            if (feature.Equals("GameType"))
            {
                foreach (Score score in scores)
                {
                    if (game.Type.Equals(score.Game.Type))
                    {
                        remainingScore.Add(score);
                    }
                }
            }
            if (feature.Equals("Location"))
            {
                foreach (Score score in scores)
                {
                    if (game.Location.Equals(score.Game.Location))
                    {
                        remainingScore.Add(score);
                    }
                }
            }
            if (feature.Equals("Opponent"))
            {
                foreach (Score score in scores)
                {
                    List<Player> players = new List<Player>();
                    foreach (Team team in game.Teams)
                    {
                        foreach (Player player in team.Players)
                        {
                            players.Add(player);
                        }
                    }
                    //de verificat
                    foreach (Player opponent in opponents)
                    {
                        if (players.Contains(opponent))
                        {
                            remainingScore.Add(score);
                        }
                    }
                }
            }            
            if (feature.Equals("Age"))
            {
                //de discutat
                remainingScore = scores;
            }
            if (feature.Equals("Referee"))
            {
                foreach (Score score in scores)
                {
                    if (game.Referee.Equals(score.Game.Referee))
                    {
                        remainingScore.Add(score);
                    }
                }
            }
            if (remainingScore.Count != 0)
            {
                return remainingScore;
            }
            else
            {
                return scores;
            }
        }
        public double GiniIndexGameType(List<Score> scores)
        {
            //Darts
            double giniIndexGameType = 0;
            List<Score> dartsX01s = new List<Score>();
            List<Score> dartsCrickets = new List<Score>();
            List<Score> fifas = new List<Score>();
            List<Score> foosballs = new List<Score>();
            List<Score> otherGames = new List<Score>();
            double giniDartsX01 = 0;
            double giniDartsCricket = 0;
            double giniFifa = 0;
            double giniFoosball = 0;
            double giniOtherGames = 0;


            foreach (Score score in scores)
            {
                if (score.Game.Type.Equals("Darts/X01"))
                {
                    dartsX01s.Add(score);
                }
                else if (score.Game.Type.Equals("Darts/Cricket"))
                {
                    dartsCrickets.Add(score);
                }
                else if (score.Game.Type.Equals("Fifa"))
                {
                    fifas.Add(score);
                }
                else if (score.Game.Type.Equals("Foosball"))
                {
                    foosballs.Add(score);
                }
                else
                {
                    otherGames.Add(score);
                }
            }
            if (dartsX01s.Count != 0)
            {
                giniDartsX01 = GiniOfSoloDartsX01(dartsX01s);
            }
            if (dartsCrickets.Count != 0)
            {
                giniDartsCricket = GiniOfSoloDartsCricket(dartsCrickets);
            }
            if (fifas.Count != 0)
            {
                giniFifa = GiniOfSoloGeneral(fifas);

            }
            if (foosballs.Count != 0)
            {
                giniFoosball = GiniOfSoloGeneral(foosballs);
            }
            if (otherGames.Count != 0)
            {
                giniOtherGames = GiniOfSoloGeneral(otherGames);
            }
            giniIndexGameType = ((double)dartsX01s.Count / scores.Count) * giniDartsX01 + ((double)dartsCrickets.Count / scores.Count) * giniDartsCricket + ((double)foosballs.Count / scores.Count) * giniFoosball
                + ((double)fifas.Count / scores.Count) * giniFifa + ((double)otherGames.Count / scores.Count) * giniOtherGames;
            return giniIndexGameType;

        }
        public double GiniIndexLocation(List<Score> scores)
        {
            double giniIndexLocation = 0;
            List<Score> parisScores = new List<Score>();
            List<Score> londonScores = new List<Score>();
            List<Score> berlinScores = new List<Score>();
            List<Score> romeScores = new List<Score>();
            List<Score> bucharestScores = new List<Score>();
            double giniParis = 0;
            double giniLondon = 0;
            double giniBerlin = 0;
            double giniRome = 0;
            double giniBucharest = 0;

            foreach (Score score in scores)
            {
                if (score.Game.Location.Equals("Paris"))
                {
                    parisScores.Add(score);
                }
                if (score.Game.Location.Equals("London"))
                {
                    londonScores.Add(score);
                }
                if (score.Game.Location.Equals("Berlin"))
                {
                    berlinScores.Add(score);
                }
                if (score.Game.Location.Equals("Rome"))
                {
                    romeScores.Add(score);
                }
                if (score.Game.Location.Equals("Bucharest"))
                {
                    bucharestScores.Add(score);
                }
            }
            if (parisScores.Count != 0)
            {
                giniParis = GiniOfSoloGeneral(parisScores);
            }
            if (londonScores.Count != 0)
            {
                giniLondon = GiniOfSoloGeneral(londonScores);
            }
            if (berlinScores.Count != 0)
            {
                giniBerlin = GiniOfSoloGeneral(berlinScores);
            }
            if (romeScores.Count != 0)
            {
                giniRome = GiniOfSoloGeneral(romeScores);
            }
            if (bucharestScores.Count != 0)
            {
                giniBucharest = GiniOfSoloGeneral(bucharestScores);
            }
            giniIndexLocation = ((double)parisScores.Count / scores.Count) * giniParis + ((double)londonScores.Count / scores.Count) * giniLondon
                + ((double)berlinScores.Count / scores.Count) * giniBerlin + ((double)romeScores.Count / scores.Count) * giniRome + ((double)bucharestScores.Count / scores.Count) * giniBucharest;
            return giniIndexLocation;

        }
        public double GiniIndexOpponents(List<Score> scores, List<Player> opponents)
        {
            List<Score> opponentScores = GetScoresOfOpponents(scores, opponents);
            double giniIndexOpponents = GiniOfSoloGeneral(opponentScores);
            return giniIndexOpponents;
        }
        public double GiniIndexAge(List<Score> scores, Player player)
        {
            double giniIndexAge = 0;
            double giniOlder = 0;
            double giniYouger = 0;
            string ageStatus;
            var AgeMap = GetScoresOfAgeStatus(scores, player);
            var youngOldValues = AgeMap.First();
            List<Score> youngerList = youngOldValues.Key;
            List<Score> olderList = youngOldValues.Value;
            if (youngerList.Count != 0)
            {
                giniYouger = GiniOfSoloGeneral(youngerList);
            }
            if (olderList.Count != 0)
            {
                giniOlder = GiniOfSoloGeneral(olderList);
            }
            giniIndexAge = ((double)youngerList.Count / scores.Count) * giniYouger + ((double)olderList.Count / scores.Count) * giniOlder;
            return giniIndexAge;
        }
        public double GiniIndexReferee(List<Score> scores)
        {
            List<Score> iacobCalinScores = new List<Score>();
            List<Score> gratianScores = new List<Score>();
            List<Score> cosminScores = new List<Score>();
            List<Score> gabiScores = new List<Score>();
            double iacobCalinGini = 0;
            double gratianGini = 0;
            double cosminGini = 0;
            double gabiGini = 0;
            double giniIndexReferee = 0;
            foreach (Score score in scores)
            {
                if (score.Game.Referee.Equals("Iacob Calin"))
                {
                    iacobCalinScores.Add(score);
                }
                if (score.Game.Referee.Equals("Gratian"))
                {
                    gratianScores.Add(score);
                }
                if (score.Game.Referee.Equals("Cosmin"))
                {
                    cosminScores.Add(score);
                }
                if (score.Game.Referee.Equals("Gabi"))
                {
                    gabiScores.Add(score);
                }
            }
            if (iacobCalinScores.Count != 0)
            {
                iacobCalinGini = GiniOfSoloGeneral(iacobCalinScores);
            }
            if (gratianScores.Count != 0)
            {
                gratianGini = GiniOfSoloGeneral(gratianScores);
            }
            if (cosminScores.Count != 0)
            {
                cosminGini = GiniOfSoloGeneral(cosminScores);
            }
            if (gabiScores.Count != 0)
            {
                gabiGini = GiniOfSoloGeneral(gabiScores);
            }
            giniIndexReferee = ((double)iacobCalinScores.Count / scores.Count) * iacobCalinGini + ((double)gratianScores.Count / scores.Count) * gratianGini
                + ((double)cosminScores.Count / scores.Count) * cosminGini + ((double)gabiScores.Count / scores.Count) * gabiGini;
            return giniIndexReferee;
        }
        public string GetBestFeature(List<string> features, List<Score> scores, List<Player> opponents,Player player)
        {
            if (scores.Count != 0)
            {
                double giniIndexGameType = 0;
                double giniIndexLocation = 0;
                double giniIndexOpponents = 0;
                double giniIndexAge = 0;
                double giniIndexReferee = 0;
            
                foreach (string feature in features)
                { 
                if (feature.Equals("GameType"))
                    {
                        giniIndexGameType = GiniIndexGameType(scores);
                    }
                    if (feature.Equals("Location"))
                    {
                        giniIndexLocation = GiniIndexLocation(scores);
                    }
                    if (feature.Equals("Opponent"))
                    {
                        giniIndexOpponents = GiniIndexOpponents(scores, opponents);
                    }
                    if (feature.Equals("Form"))
                    {
                        //mai gandim
                    }
                    if (feature.Equals("ChoosenTeam"))
                    {
                        //intrebam daca mai adauga un field
                    }
                    if (feature.Equals("Age"))
                    {
                        giniIndexAge = GiniIndexAge(scores, player);
                    }
                    if (feature.Equals("Referee"))
                    {
                        giniIndexReferee = GiniIndexReferee(scores);
                    }

                }//eliminam cele care sunt 0
                var giniIndexMap = new Dictionary<string, double>();
               
                giniIndexMap.Add("GameType",giniIndexGameType);
                giniIndexMap.Add("Location",giniIndexLocation);
                giniIndexMap.Add("Opponents",giniIndexOpponents);
                giniIndexMap.Add("Age",giniIndexAge);
                giniIndexMap.Add("Referee",giniIndexReferee);

                KeyValuePair<string, double> choosenGiniIndex = new KeyValuePair<string, double>("empty",1);
                foreach (KeyValuePair<string,double> giniIndex in giniIndexMap)
                {
                    if ((choosenGiniIndex.Value > giniIndex.Value) && (giniIndex.Value != 0))
                    {
                        choosenGiniIndex = giniIndex;
                    }
                }
                return choosenGiniIndex.Key;
            }
            return "";
        }
        public Dictionary<List<Score>,List<Score>> GetScoresOfAgeStatus(List<Score> scores,Player player)
        {
            var AgeMap = new Dictionary<List<Score>, List<Score>>();
            List<Score> youngerScores = new List<Score>();
            List<Score> olderScores = new List<Score>();
            string ageStatus = "";
            foreach (Score score in scores)
            {
                List<Player> opponents = GetOpponentsOfGame(score,player);
                if (opponents.Count > score.Game.Teams.Count)
                {
                    foreach (Team team in score.Game.Teams)
                    {
                        if (CheckPlayerInTeam(team, player))
                        {
                            ageStatus = GetTeamAgeStatus(opponents, team);
                            break;
                        }
                    }
                }
                else
                {
                    ageStatus = GetAgeStatus(opponents, player);
                }
                if (ageStatus.Equals("younger"))
                {
                    youngerScores.Add(score);
                }
                else
                {
                    olderScores.Add(score);
                }
            }
            AgeMap.Add(youngerScores, olderScores);
            return AgeMap;
        }
        public List<Player> GetOpponentsOfGame(Score score,Player player)
        {
            
            List<Player> opponents = new List<Player>();
            foreach (Team team in score.Game.Teams)
            {
                if (team.Players.Count != 1)
                {
                    if (!(CheckPlayerInTeam(team, player)))
                    {
                        foreach (Player p in team.Players)
                        {
                            opponents.Add(p);
                        }
                    }
                }
                else
                {
                    Player opponent = team.Players[0];
                    if (!opponent.Id.Equals(player.Id))
                    {
                        opponents.Add(opponent);
                    }
                }
            }
            return opponents;
        }
        public bool CheckPlayerInTeam(Team team,Player player)
        {
            foreach (Player playerT in team.Players)
            {
                if (playerT.Id.Equals(player.Id))
                {
                    return true;
                }
            }
            return false;
        }
        public string GetAgeStatus(List<Player> opponents, Player player)
        {
            int opponentsAge = 0;
            foreach (Player opponent in opponents)
            {
                opponentsAge = opponent.Age;
            }
            opponentsAge = opponentsAge / opponents.Count;
            if (player.Age > opponentsAge)
            {
                return "older";
            }
            else {
                return "younger";
            }
        }
        public string GetTeamAgeStatus(List<Player> opponents, Team team)
        {
            double teamAge = 0;
            foreach (Player teamPlayer in team.Players)
            {
                teamAge = teamAge + teamPlayer.Age;
            }
            teamAge = teamAge / team.Players.Count;
            int opponentsAge = 0;
            foreach (Player opponent in opponents)
            {
                opponentsAge = opponent.Age;
            }
            opponentsAge = opponentsAge / opponents.Count;
            if (teamAge > opponentsAge)
            {
                return "older";
            }
            else
            {
                return "younger";
            }
        }
        public double GiniOfSoloDartsX01(List<Score> dartsX01s)
        {
            //for every game find enemy score
            int nrOfWins = 0;
            int nrOfLoss = 0;
            foreach (Score score in dartsX01s) {
                if (score.Value == 1)
                {
                    nrOfWins++;
                }
                else
                {
                    nrOfLoss++;
                }
            }
            double giniOfDartsX01 = 1 - Math.Pow((double)nrOfWins / dartsX01s.Count, 2)-Math.Pow((double)nrOfLoss / dartsX01s.Count, 2);
            return giniOfDartsX01;
        }
        public double GiniOfSoloDartsCricket(List<Score> dartsCrickets)
        {
            //for every game find enemy score
            int nrOfWins = 0;
            int nrOfLoss = 0;
            foreach (Score score in dartsCrickets)
            {
                if (score.Value == 1)
                {
                    nrOfWins++;
                }
                else
                {
                    nrOfLoss++;
                }
            }
            double giniOfDartsX01 = 1 - Math.Pow((double)nrOfWins / dartsCrickets.Count, 2) - Math.Pow((double)nrOfLoss / dartsCrickets.Count, 2);
            return giniOfDartsX01;
        }
        public double GiniOfSoloGeneral(List<Score> scores)
        {
            if (scores.Count == 0)
            {
                return 0;
            }
            int nrOfWins = 0;
            int nrOfLoss = 0;
            foreach (Score score in scores)
            {
                if (WinTheGame(score))
                {
                    nrOfWins++;
                }
                else {
                    nrOfLoss++;
                }
            }
            double m = (double)nrOfWins / scores.Count;
            double giniOfGeneral = 1 - Math.Pow((double)nrOfWins / scores.Count, 2) - Math.Pow((double)nrOfLoss / scores.Count, 2);
            return giniOfGeneral;
        }
        public List<Score> GetScoresOfOpponents(List<Score> scores, List<Player> opponents)
        {
            //de verificat containul de la lista
            List<Score> scoresWithOpponents = new List<Score>();
            foreach (Score score in scores)
            {
                List<Team> teams = score.Game.Teams;
                foreach (Team team in teams)
                {
                    int scoresSize = scoresWithOpponents.Count;
                    foreach (Player opponent in opponents)
                    {
                        if (CheckIfScoreExist(scoresWithOpponents,score))
                        {
                            break;
                        }
                        if (CheckIfPlayerExist(team.Players,opponent))
                        {
                            scoresWithOpponents.Add(score);
                            break;
                        }
                   
                    }
                    if (scoresSize != scoresWithOpponents.Count)
                    {
                        break;
                    }
                }
            }
            return scoresWithOpponents;
        }
        public bool CheckIfPlayerExist(List<Player> players, Player player)
        {
            foreach (Player p in players)
            {
                if (p.Id.Equals(player.Id))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckIfScoreExist(List<Score> scores, Score score)
        {
            foreach (Score s in scores)
            {
                if (s.Id.Equals(score.Id))
                {
                    return true;
                }
            }
            return false;

        }
        public bool WinTheGame(Score score)
        {
            Game game = score.Game;
            List<Score> scores = scoreRepository.GetScoresForGame(game);
            foreach (Score enemyScore in scores)
            {
                if (enemyScore.Id != score.Id)
                {
                    if (enemyScore.Value > score.Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public string MakeDecision(List<Score> scores)
        {
            int nr_wins = 0;
            int nr_defeats = 0;
            foreach (Score score in scores)
            {
                if (WinTheGame(score) == true)
                {
                    nr_wins++;
                }
                else {
                    nr_defeats++;
                }
            }
            if (nr_wins > nr_defeats)
            {
                return "win";
            }
            else if (nr_wins == nr_defeats)
            {
                return "equality";
            }
            else
            {
                return "defeat";
            }
        }
    }
}