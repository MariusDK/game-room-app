using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GameRoomApp.Util
{
    public class PredictionGame
    {
        private string _response;
        public byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open,FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        public async Task<string> MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", "3e9438bd78174aaa8dd27cd01641d2aa");
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/d4bdc768-1bf6-4f25-a2b9-5f5780c4e4c9/image?iterationId=36fa6759-e3b0-4675-a12e-3e80db312319";
            HttpResponseMessage response;

            //byte[] byteData = GetImageAsByteArray(imageFilePath);
            byte[] byteData = Convert.FromBase64String(imageFilePath);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response =  await client.PostAsync(url,content);
                Console.WriteLine( await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsStringAsync();
            }
        }
        public string ParseJsonToGetPrediction(string perdictionJson)
        {
            JObject o = JObject.Parse(perdictionJson);
            JToken bestPrediction = o.Last.First.First.Last.First;
            JToken bestPredictionValue = o.Last.First.First.First.First;
            string bestPredictionResult = bestPrediction.ToString()+" (probability "+bestPredictionValue.ToString()+")";
            return bestPredictionResult;
        }
    }
}
