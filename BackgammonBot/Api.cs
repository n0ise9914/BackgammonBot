using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackgammonBot
{
    public class Api
    {
        private string _url = "http://localhost/bigleagueapi/public/api/";
        private static Api _instance;
        private string _authKey;

        public static Api GetInstance()
        {
            return _instance ?? (_instance = new Api());
        }

        private Api()
        {
        }

        public dynamic Send(string method)
        {
            dynamic request = new JObject();
            request.json = true;
            return Send(method, request);
        }

        public dynamic Send(string method, dynamic jObject)
        {
            WebRequest request = WebRequest.Create(_url + method);
            request.Method = "POST";
            string postData = JsonConvert.SerializeObject(jObject);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            if (_authKey != null)
                request.Headers.Add("Auth", _authKey);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream ?? throw new Exception("1516"));
            string responseFromServer = reader.ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject<JObject>(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return data;
        }

        public dynamic Verify(long number)
        {
            dynamic request = new JObject();
            request.mobile = number;
            dynamic response = Send("verify", request);
            _authKey = response.auth;
            return response;
        }

        public dynamic GetProfile()
        {
            dynamic response = Send("getProfile");
            return response;
        }

        public dynamic MatchRequest(string game)
        {
            dynamic request = new JObject();
            request.game = game;
            dynamic response = Send("matchRequest", request);
            return response;
        }

        public dynamic GetStatus(int matchId, int lastMove)
        {
            dynamic request = new JObject();
            request.match = matchId;
            request.last_move = lastMove;
            dynamic response = Send("getStatus", request);
            return response;
        }

        public dynamic GetDice(string game, int matchId)
        {
            dynamic request = new JObject();
            request.match = matchId;
            request.game = game;
            dynamic response = Send("getDice", request);
            return response;
        }
        
        public dynamic MatchPlay(dynamic board, int matchId,int turn)
        {
            dynamic request = new JObject();        
            request.board = board;
            request.match = matchId;
            request.turn = turn;
            Console.WriteLine( JsonConvert.SerializeObject(request));
            dynamic response = Send("matchPlay", request);
            return response;
        }
    }
}