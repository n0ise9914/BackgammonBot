using System;
using System.Diagnostics;
using System.Threading;
using BackgammonBot.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackgammonBot
{
    public class BackgammonBot
    {
        private Api _api;
        private int _lastMove;
        private int _matchId;
        private Backgammon _backgammon;

        public BackgammonBot(Api api)
        {
            _backgammon = new Backgammon();
            _api = api;
        }


        public void Start()
        {
            JoinMatch();
            GetStatus();
        }

        private void JoinMatch()
        {
            while (true)
            {
                dynamic matchRequest = _api.MatchRequest("backgammon");
                Console.WriteLine("MatchRequest: " + matchRequest.message);
                if (matchRequest.message == "Join match")
                {
                    _matchId = matchRequest.matchId;
                    break;
                }
                Thread.Sleep(2000);
            }
        }

        private void GetStatus()
        {
            while (true)
            {
                dynamic data = _api.GetStatus(_matchId, _lastMove);
                String str = JsonConvert.SerializeObject(data);
                Debug.WriteLine(str);
                if (data.last_move != null)
                    _lastMove = data.last_move;
                if (data.match.turn == data.match.me)
                {
                    Play(data);
                }
                else if (data.match.status == "ended")
                {
                    Finish();
                    return;
                }
                Thread.Sleep(2000);
            }
        }

        private void Finish()
        {
            Console.WriteLine("game is over.");
        }

        private void Play(dynamic data)
        {
            Thread.Sleep(1000);       
            JArray moves = _backgammon.GetMoves(_api,data,_matchId);
            for (int i = 0; i < moves.Count; i++)
            {
                Thread.Sleep(2000);     
                int turn = Convert.ToInt32(data.match.me);
                if (i == moves.Count - 1)
                    turn = _backgammon.Opponent(turn);
                _api.MatchPlay(moves[i], _matchId, turn);       
            }
        }
    }
}