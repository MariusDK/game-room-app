using Newtonsoft.Json.Linq;

namespace GameRoomApp.Util
{
    class PredictionJsonParser
    {
        public string ParseJsonToGetPrediction(string perdictionJson)
        {
            JObject o = JObject.Parse(perdictionJson);
            string bestPrediction = (string)o.SelectToken("Predictions.Tag");
            return bestPrediction;
        }
    }
}
