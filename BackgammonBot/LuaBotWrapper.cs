using System;
using System.Collections.Generic;
using BackgammonBot.Utils;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackgammonBot
{
    public class Backgammon : LuaWrapper
    {
        public Backgammon() : base("src.bot")
        {
            
        }

        public JObject CreateBoard()
        {
            string board =
                "{\"checkers\":[-2,0,0,0,0,5,0,3,0,0,0,-5,5,0,0,0,-3,0,-5,0,0,0,0,2],\"hitCheckers\":[0,0],\"bearingOffCheckers\":[0,0]}";
            return (JObject) JsonConvert.DeserializeObject(board);
        }

        public string FindAllPossibleMoves(object me, object board, object dice)
        {
            return Call("findAllPossibleMoves", Convert.ToDouble(me),
                JsonConvert.SerializeObject(board), JsonConvert.SerializeObject(dice)).String;
        }

        public List<Tuple<object, object>> SelectBestMove(string possibleMoves, object me, object board)
        {
            dynamic resopnse = JsonConvert.DeserializeObject(Call("selectBestMove", possibleMoves, Convert.ToDouble(me),
                JsonConvert.SerializeObject(board)).String);
            List<Tuple<object, object>> moves = new List<Tuple<object, object>>();
            for (int i = 0; i < resopnse.source.Count; i++)
            {
                moves.Add(new Tuple<object, object>(resopnse.source[i].ToObject<object>(),
                    resopnse.destination[i].ToObject<object>()));
            }

            return moves;
        }

        public string Move(object me, object board, object source, object destination)
        {
            return Call("doMove", Convert.ToDouble(me), JsonConvert.SerializeObject(board), source, destination).String;
        }

        public int Opponent(int player)
        {
            return player == 1 ? 2 : 1;
        }
        
        public JArray GetMoves(Api api, dynamic data, int matchId)
        {
            JArray moves = new JArray();
            JObject board = data.board.Count == 0 ? CreateBoard() : data.board[data.board.Count - 1];
            dynamic dices = api.GetDice("backgammon", matchId);
            string possibleMoves = FindAllPossibleMoves(data.match.me, board, dices.dice);
            List<Tuple<object, object>> bestMoves = SelectBestMove(possibleMoves, data.match.me, board);
            dynamic previousBoard = board;
            foreach (var move in bestMoves)
            {
                previousBoard.Remove("previous");
                dynamic newBoard = JsonConvert.DeserializeObject(Move(data.match.me, previousBoard, move.Item1, move.Item2));
                newBoard.move = new JArray {move.Item1, move.Item2};
                newBoard.move.Add(data.match.me);
                newBoard.previous =previousBoard;
                moves.Add(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(newBoard)));
                previousBoard = newBoard;
            }
            return moves;
        }
    }
}