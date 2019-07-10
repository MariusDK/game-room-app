using GameRoomApp.DataModel;
using GameRoomApp.providers.ScoreRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameRoomApp.Util.CART
{
    public class DecisionTreeTeam
    {

        public Node root { get; set; }
        public string prediction { get; set; }
        public double procentualPrediction { get; set; }
        public IScoreRepository scoreRepository;

        public DecisionTreeTeam(Node root, IScoreRepository scoreRepository)
        {
            this.prediction = null;
            this.root = root;
            this.scoreRepository = scoreRepository;
        }
        public DecisionTreeTeam(List<Score> scores, List<string> features, List<Team> opponents, Team team, Game game, IScoreRepository scoreRepository)
        {
            this.scoreRepository = scoreRepository;
            string bestFeature = GetBestFeature(features, scores, opponents, team);
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
                if (scores.Count < 2)
                {
                    break;
                }
                if (nodes.Count != 0)
                {
                    currentNode = nodes.First();
                    bestFeature = GetBestFeature(features, scores, opponents, team);
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
            else
            {
                prediction = "equality";
            }

        }
        public List<Score> GetScoresAfterFeature(Game game, string feature, List<Score> scores, List<Team> opponents)
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
                    List<Team> teams = new List<Team>();
                    foreach (Team team in game.Teams)
                    {
                        teams.Add(team);
                    }
                    //de verificat
                    foreach (Team opponent in opponents)
                    {
                        if (teams.Contains(opponent))
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
        public double GiniIndexOpponents(List<Score> scores, List<Team> opponents)
        {
            List<Score> opponentScores = GetScoresOfOpponents(scores, opponents);
            double giniIndexOpponents = GiniOfSoloGeneral(opponentScores);
            return giniIndexOpponents;
        }
        public double GiniIndexAge(List<Score> scores, Team team)
        {
            double giniIndexAge = 0;
            double giniOlder = 0;
            double giniYouger = 0;
            string ageStatus="";
            var AgeMap = GetScoresOfAgeStatus(scores, team);
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
        public string GetBestFeature(List<string> features, List<Score> scores, List<Team> opponents, Team team)
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
                        giniIndexAge = GiniIndexAge(scores, team);
                    }
                    if (feature.Equals("Referee"))
                    {
                        giniIndexReferee = GiniIndexReferee(scores);
                    }

                }//eliminam cele care sunt 0
                var giniIndexMap = new Dictionary<string, double>();

                giniIndexMap.Add("GameType", giniIndexGameType);
                giniIndexMap.Add("Location", giniIndexLocation);
                giniIndexMap.Add("Opponents", giniIndexOpponents);
                giniIndexMap.Add("Age", giniIndexAge);
                giniIndexMap.Add("Referee", giniIndexReferee);

                KeyValuePair<string, double> choosenGiniIndex = new KeyValuePair<string, double>("empty", 1);
                foreach (KeyValuePair<string, double> giniIndex in giniIndexMap)
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
        public Dictionary<List<Score>, List<Score>> GetScoresOfAgeStatus(List<Score> scores, Team team)
        {
            var AgeMap = new Dictionary<List<Score>, List<Score>>();
            List<Score> youngerScores = new List<Score>();
            List<Score> olderScores = new List<Score>();
            string ageStatus = "";
            foreach (Score score in scores)
            {
                List<Team> opponents = GetOpponentsOfGame(score, team);
                ageStatus = GetTeamAgeStatus(opponents, team);
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
        public List<Team> GetOpponentsOfGame(Score score, Team teamOur)
        {
            List<Team> opponents = new List<Team>();
            foreach (Team team in score.Game.Teams)
            {
                if (team.Id != teamOur.Id)
                {
                    opponents.Add(team);

                }
            }
            return opponents;
        }
        
        public string GetTeamAgeStatus(List<Team> opponents, Team team)
        {
            double teamAge = 0;
            foreach (Player teamPlayer in team.Players)
            {
                teamAge = teamAge + teamPlayer.Age;
            }
            teamAge = teamAge / team.Players.Count;
            int opponentsAge = 0;
            foreach (Team opponent in opponents)
            {
                int opTeamAge = 0;
                foreach (Player opPlayer in  opponent.Players)
                {
                    opTeamAge = opTeamAge + opPlayer.Age;
                }

                opponentsAge = opponentsAge + opTeamAge/opponent.Players.Count;
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
            foreach (Score score in dartsX01s)
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
            double giniOfDartsX01 = 1 - Math.Pow((double)nrOfWins / dartsX01s.Count, 2) - Math.Pow((double)nrOfLoss / dartsX01s.Count, 2);
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
                else
                {
                    nrOfLoss++;
                }
            }
            double m = (double)nrOfWins / scores.Count;
            double giniOfGeneral = 1 - Math.Pow((double)nrOfWins / scores.Count, 2) - Math.Pow((double)nrOfLoss / scores.Count, 2);
            return giniOfGeneral;
        }
        public List<Score> GetScoresOfOpponents(List<Score> scores, List<Team> opponents)
        {
            //de verificat containul de la lista
            List<Score> scoresWithOpponents = new List<Score>();
            foreach (Score score in scores)
            {
                List<Team> teams = score.Game.Teams;
                foreach (Team team in teams)
                {
                    if (opponents.Contains(team))
                    {
                        scoresWithOpponents.Add(score);
                    }
                }
            }
            return scoresWithOpponents;
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
                else
                {
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
        // --------------------------------------------------------REGRETION PART
        public DecisionTreeTeam(List<Score> scores, List<string> features, List<Team> opponents, Team team, Game game, IScoreRepository scoreRepository, string a)
        {
            if (scores.Count == 0)
            {
                procentualPrediction = 50;
            }
            else
            {
                if (scores.Count == 1)
                {
                    procentualPrediction = scores[0].ChanceOfVictory;
                }
                else
                {
                    this.scoreRepository = scoreRepository;
                    string bestFeature = GetBestFeatureRegression(features, scores, opponents, team);
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
                        if ((scores.Count < 3) || (CoeficientVariation(scores) < 10))
                        {
                            break;
                        }
                        if (nodes.Count != 0)
                        {
                            currentNode = nodes.First();
                            bestFeature = GetBestFeatureRegression(features, scores, opponents, team);
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
                        procentualPrediction = CalculateRegressionPrediction(scores);
                    }
                }
            }
        }
        public string GetBestFeatureRegression(List<string> features, List<Score> scores, List<Team> opponents, Team team)
        {
            if (scores.Count != 0)
            {
                double SDRGameType = 0;
                double SDRLocation = 0;
                double SDROpponents = 0;
                double SDRAge = 0;
                double SDRReferee = 0;
                double STarget = StandardDeviationOfTarget(scores);

                foreach (string feature in features)
                {
                    if (feature.Equals("GameType"))
                    {
                        //calculam standard deviation GameType
                        //facem diferenta si obtine reducerea deviatie standard
                        double sdGameType = StandardDeviationGameType(scores);
                        SDRGameType = STarget - sdGameType;

                    }
                    if (feature.Equals("Location"))
                    {
                        double sdLocation = StandardDeviationLocation(scores);
                        SDRLocation = STarget - sdLocation;
                    }
                    if (feature.Equals("Opponents"))
                    {
                        double sdOpponents = StandardDeviationOpponents(scores, opponents);
                        SDROpponents = STarget - sdOpponents;
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
                        double sdAge = StandardDeviationAge(scores, team);
                        SDRAge = STarget - sdAge;
                    }
                    if (feature.Equals("Referee"))
                    {
                        double sdReferee = StandardDeviationReferee(scores);
                        SDRReferee = STarget - sdReferee;
                    }

                }//eliminam cele care sunt 0
                var SDRMap = new Dictionary<string, double>();

                SDRMap.Add("GameType", SDRGameType);
                SDRMap.Add("Location", SDRLocation);
                SDRMap.Add("Opponents", SDROpponents);
                SDRMap.Add("Age", SDRAge);
                SDRMap.Add("Referee", SDRReferee);

                KeyValuePair<string, double> choosenSDR = new KeyValuePair<string, double>("empty", 0);
                foreach (KeyValuePair<string, double> sdr in SDRMap)
                {
                    if (sdr.Value > choosenSDR.Value)
                    {
                        choosenSDR = sdr;
                    }
                }
                return choosenSDR.Key;
            }
            return "";

        }
        public double StandardDeviationOfTarget(List<Score> scores)
        {
            double standardDeviationTarget = 0;
            double medie = MedieScores(scores);
            foreach (Score score in scores)
            {
                double val = score.ChanceOfVictory - medie;
                standardDeviationTarget = standardDeviationTarget + Math.Pow(val, 2);
            }
            standardDeviationTarget = standardDeviationTarget / scores.Count;
            double finalSDT = Math.Sqrt(standardDeviationTarget);
            return finalSDT;
        }
        public double MedieScores(List<Score> scores)
        {
            double avg = 0;
            foreach (Score score in scores)
            {
                avg = avg + score.ChanceOfVictory;
            }
            return avg / scores.Count;
        }
        public double StandardDeviationGameType(List<Score> scores)
        {
            //Darts
            double sdGameType = 0;
            List<Score> dartsX01s = new List<Score>();
            List<Score> dartsCrickets = new List<Score>();
            List<Score> fifas = new List<Score>();
            List<Score> foosballs = new List<Score>();
            List<Score> otherGames = new List<Score>();
            double sdDartsX01 = 0;
            double sdDartsCricket = 0;
            double sdFifa = 0;
            double sdFoosball = 0;
            double sdOtherGames = 0;


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
                sdDartsX01 = StandardDeviationOfTarget(dartsX01s);
            }
            if (dartsCrickets.Count != 0)
            {
                sdDartsCricket = StandardDeviationOfTarget(dartsCrickets);
            }
            if (fifas.Count != 0)
            {
                sdFifa = StandardDeviationOfTarget(fifas);

            }
            if (foosballs.Count != 0)
            {
                sdFoosball = StandardDeviationOfTarget(foosballs);
            }
            if (otherGames.Count != 0)
            {
                sdOtherGames = StandardDeviationOfTarget(otherGames);
            }
            sdGameType = ((double)dartsX01s.Count / scores.Count) * sdDartsX01 + ((double)dartsCrickets.Count / scores.Count) * sdDartsCricket + ((double)foosballs.Count / scores.Count) * sdFoosball
                + ((double)fifas.Count / scores.Count) * sdFifa + ((double)otherGames.Count / scores.Count) * sdOtherGames;


            return sdGameType;
        }
        public double StandardDeviationLocation(List<Score> scores)
        {
            double sdLocation = 0;
            List<Score> parisScores = new List<Score>();
            List<Score> londonScores = new List<Score>();
            List<Score> berlinScores = new List<Score>();
            List<Score> romeScores = new List<Score>();
            List<Score> bucharestScores = new List<Score>();
            double sdParis = 0;
            double sdLondon = 0;
            double sdBerlin = 0;
            double sdRome = 0;
            double sdBucharest = 0;

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
                sdParis = StandardDeviationOfTarget(parisScores);
            }
            if (londonScores.Count != 0)
            {
                sdLondon = StandardDeviationOfTarget(londonScores);
            }
            if (berlinScores.Count != 0)
            {
                sdBerlin = StandardDeviationOfTarget(berlinScores);
            }
            if (romeScores.Count != 0)
            {
                sdRome = StandardDeviationOfTarget(romeScores);
            }
            if (bucharestScores.Count != 0)
            {
                sdBucharest = StandardDeviationOfTarget(bucharestScores);
            }
            sdLocation = ((double)parisScores.Count / scores.Count) * sdParis + ((double)londonScores.Count / scores.Count) * sdLondon
                + ((double)berlinScores.Count / scores.Count) * sdBerlin + ((double)romeScores.Count / scores.Count) * sdRome + ((double)bucharestScores.Count / scores.Count) * sdBucharest;
            return sdLocation;

        }
        public double StandardDeviationOpponents(List<Score> scores, List<Team> opponents)
        {
            List<Score> opponentScores = GetScoresOfOpponents(scores, opponents);
            double sdOpponents = StandardDeviationOfTarget(opponentScores);
            return sdOpponents;
        }
        public double StandardDeviationAge(List<Score> scores, Team team)
        {
            double sdAge = 0;
            double sdOlder = 0;
            double sdYouger = 0;
            string ageStatus;
            var AgeMap = GetScoresOfAgeStatus(scores, team);
            var youngOldValues = AgeMap.First();
            List<Score> youngerList = youngOldValues.Key;
            List<Score> olderList = youngOldValues.Value;
            if (youngerList.Count != 0)
            {
                sdYouger = StandardDeviationOfTarget(youngerList);
            }
            if (olderList.Count != 0)
            {
                sdOlder = StandardDeviationOfTarget(olderList);
            }
            sdAge = ((double)youngerList.Count / scores.Count) * sdYouger + ((double)olderList.Count / scores.Count) * sdOlder;
            return sdAge;
        }
        public double StandardDeviationReferee(List<Score> scores)
        {
            List<Score> iacobCalinScores = new List<Score>();
            List<Score> gratianScores = new List<Score>();
            List<Score> cosminScores = new List<Score>();
            List<Score> gabiScores = new List<Score>();
            double sdIacobCalin = 0;
            double sdGratian = 0;
            double sdCosmin = 0;
            double sdGabi = 0;
            double sdReferee = 0;
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
                sdIacobCalin = StandardDeviationOfTarget(iacobCalinScores);
            }
            if (gratianScores.Count != 0)
            {
                sdGratian = StandardDeviationOfTarget(gratianScores);
            }
            if (cosminScores.Count != 0)
            {
                sdCosmin = StandardDeviationOfTarget(cosminScores);
            }
            if (gabiScores.Count != 0)
            {
                sdGabi = StandardDeviationOfTarget(gabiScores);
            }
            sdReferee = ((double)iacobCalinScores.Count / scores.Count) * sdIacobCalin + ((double)gratianScores.Count / scores.Count) * sdGratian
                + ((double)cosminScores.Count / scores.Count) * sdCosmin + ((double)gabiScores.Count / scores.Count) * sdGabi;
            return sdReferee;
        }
        public double CoeficientVariation(List<Score> featureScore)
        {
            double CV = (StandardDeviationOfTarget(featureScore) / featureScore.Count) * 100;
            return CV;
        }
        public double CalculateRegressionPrediction(List<Score> scores)
        {
            double regressionPrediction = 0;
            foreach (Score score in scores)
            {
                regressionPrediction = regressionPrediction + score.ChanceOfVictory;
            }
            return regressionPrediction / scores.Count;
        }

    }
}
